# React + TypeScript Migration Plan (Strangler Fig)

Migrate SampleStore from server-rendered ASP.NET Core Razor Pages to a full **React + TypeScript SPA** backed by **ASP.NET Core Minimal APIs**, using the **Strangler Fig** pattern: the new SPA grows alongside the existing Razor Pages app, one route at a time, until the old UI is fully "strangled" and removed.

---

## 1. Guiding principles

- **One small thing per step.** Every step below is scoped to fit comfortably in a ~30K-token working context: it touches only a handful of small files, builds and runs independently, and ends with a concrete verification. If a step starts to feel large, split it (the suffixes `a`/`b` show natural split points).
- **Always shippable.** After every step the app builds (`dotnet build`), runs, and all existing functionality still works. Each step is a single commit and is trivially revertable.
- **Strangler route-flip.** Razor Pages have *explicit* routes, so they always win over the SPA fallback. The SPA is served by `MapFallbackToFile("index.html")`, which only catches paths **not** claimed by a Razor Page. To migrate a route you (1) build the React page for the *same URL*, then (2) **delete the Razor `.cshtml` + `.cshtml.cs`** so that URL falls through to the SPA. URLs never change; the two systems coexist for the whole migration.
- **Reuse the existing cookie session.** No new auth system. The SPA authenticates with the *same* ASP.NET cookie the Razor app already issues. During migration, users can still log in via the existing Razor Identity pages while the SPA reads auth state from `/api/auth/me`. Identity is migrated **last**, when everything else already works.
- **Migrate lowest-risk first:** scaffolding → read-only public pages → admin CRUD → identity/account → cutover/cleanup.

## 2. Target architecture

```
SampleStore.Host          → composition root; serves API + SPA static files + fallback
SampleStore.Api  (NEW)    → Minimal API endpoint modules + request/response DTOs
SampleStore.Web  (NEW use)→ Vite + React + TypeScript SPA (existing placeholder folder)
SampleStore.Data*         → unchanged (UnitOfWork/Repository reused by the API)
SampleStore.Services.*    → unchanged (Identity/Email reused by the API)
SampleStore.UI            → shrinks to nothing as pages migrate; eventually removed
```

- **Auth:** cookie auth (existing). API endpoints return **JSON 401/403** instead of redirecting. Unsafe methods protected by **antiforgery** header `X-CSRF-TOKEN`.
- **Dev:** Vite dev server (HMR) proxies `/api`, `/openapi`, and `/identity` (OAuth callbacks) to Kestrel. **Prod:** `vite build` output is served by the Host as static files; `dotnet publish` runs the build via an MSBuild target.
- **Types:** TypeScript request/response types are generated from the API's OpenAPI document (`openapi-typescript`) — single source of truth, no hand-written drift.

## 3. Conventions for new code

- New NuGet packages go in **`Directory.Packages.props`** (central package mgmt); reference them version-free in the `.csproj`.
- New projects are added to **`SampleStore.slnx`**.
- Honor `Nullable=enable` and `TreatWarningsAsErrors=true` — new code must be warning-clean.
- `SampleStore.Web` is a plain folder (no `.csproj`); `node_modules/` and `dist/` are git-ignored.
- Endpoint modules implement a small `IEndpointModule` and self-register, mirroring the existing "each layer registers itself" DI convention.

## 4. Verification baseline (run after most steps)

```bash
dotnet build SampleStore.slnx
dotnet run --project SampleStore.Host/SampleStore.Host.csproj
# in SampleStore.Web:  npm run dev   (and/or)  npm run build
```

---

# Phase 0 — Scaffolding (no user-visible change)

### Step 0.1 — Scaffold the Vite React+TS app
- **Goal:** A buildable React+TypeScript app in `SampleStore.Web`.
- **Touch:** `SampleStore.Web/{package.json, vite.config.ts, tsconfig.json, index.html, src/main.tsx, src/App.tsx, .gitignore}`.
- **Do:** `npm create vite@latest` (react-ts template) into the folder; set strict TS; add `.gitignore` for `node_modules/`, `dist/`.
- **Verify:** `npm install && npm run build` succeeds; `npm run dev` serves a page.

### Step 0.2 — Configure the dev proxy
- **Goal:** SPA dev server talks to the .NET backend.
- **Touch:** `vite.config.ts`.
- **Do:** Proxy `/api`, `/openapi`, `/identity` → `https://localhost:<kestrel port>` with `changeOrigin` and `secure:false`; set a fixed dev port.
- **Verify:** From the running SPA, a fetch to `/api/ping` (added in 0.7) reaches Kestrel.

### Step 0.3 — Lint/format/strictness
- **Goal:** Consistent TS quality gates.
- **Touch:** `eslint.config.js`, `.prettierrc`, `tsconfig.json`, `package.json` scripts.
- **Do:** Add ESLint (typescript-eslint, react-hooks), Prettier, `"lint"`/`"format"` scripts; enable `strict`, `noUncheckedIndexedAccess`.
- **Verify:** `npm run lint` passes on the scaffold.

### Step 0.4 — Create the `SampleStore.Api` project
- **Goal:** Empty endpoint-hosting class library.
- **Touch:** `SampleStore.Api/SampleStore.Api.csproj`, `SampleStore.Api/Endpoints/IEndpointModule.cs`, `SampleStore.Api/Extensions/ServiceCollectionExtensions.cs` (`AddApiEndpoints`, `MapApiEndpoints`), `SampleStore.slnx`.
- **Do:** Reference `SampleStore.Data`, `SampleStore.Data.Extensions`, `SampleStore.Data.Entities`, `SampleStore.Services.Identity`. Define `IEndpointModule { void Map(IEndpointRouteBuilder app); }` and a reflection/DI registration helper. Add project to `.slnx`.
- **Verify:** `dotnet build SampleStore.slnx` succeeds.

### Step 0.5 — Wire API + SPA static hosting into the Host
- **Goal:** Host serves API endpoints and the SPA fallback **without** breaking Razor.
- **Touch:** `SampleStore.Host/Program.cs`, `SampleStore.Host.csproj` (ref `SampleStore.Api`).
- **Do:** Call `AddApiEndpoints()`; after `MapRazorPages()` add `MapApiEndpoints()`, `UseStaticFiles` for the SPA build dir, and `MapFallbackToFile("index.html")` marked **AllowAnonymous**. (No routes migrated yet, so fallback only catches truly unmatched paths.)
- **Verify:** App runs; all existing Razor pages still work; an unknown URL serves the SPA shell.

### Step 0.6 — Add OpenAPI + API explorer
- **Goal:** Live API schema for type generation and manual testing.
- **Touch:** `Directory.Packages.props`, `SampleStore.Host.csproj` (+`Microsoft.AspNetCore.OpenApi`), `Program.cs`.
- **Do:** `AddOpenApi()`; map `/openapi/v1.json` and a Scalar/Swagger UI in Development only.
- **Verify:** `/openapi/v1.json` loads in dev.

### Step 0.7 — Add a ping endpoint (first endpoint module)
- **Goal:** Prove the endpoint-module pattern + dev proxy end-to-end.
- **Touch:** `SampleStore.Api/Endpoints/SystemEndpoints.cs`.
- **Do:** `GET /api/ping → { status: "ok" }` via `IEndpointModule`.
- **Verify:** Reachable from browser and through the Vite proxy.

---

# Phase 1 — Read-only Products API

### Step 1.1 — Product read DTOs
- **Touch:** `SampleStore.Api/Dtos/Products/{ProductListItemDto.cs, ProductDetailsDto.cs}`.
- **Do:** List item = `Id, Name, Price, Quantity, HasPhoto`; details adds `Description`. Photo exposed as URL, not bytes.
- **Verify:** Build.

### Step 1.2 — Product → DTO mapping
- **Touch:** `SampleStore.Api/Mapping/ProductMapper.cs`.
- **Do:** Static extension methods `ToListItemDto()`, `ToDetailsDto()`.
- **Verify:** Build.

### Step 1.3 — `GET /api/products`
- **Touch:** `SampleStore.Api/Endpoints/ProductEndpoints.cs`.
- **Do:** List all products via `IUnitOfWork`/`IQueryMaterializer`; project to list DTOs.
- **Verify:** Returns JSON array.

### Step 1.4 — `GET /api/products/{id}`
- **Touch:** `ProductEndpoints.cs`.
- **Do:** Return details DTO; `404` if not found.
- **Verify:** Valid id → 200; bogus id → 404.

### Step 1.5 — `GET /api/products/{id}/photo`
- **Touch:** `ProductEndpoints.cs`.
- **Do:** Return `File(photo.Image, photo.MimeType)`; `404` if no photo. (Replaces `Pages/Products/Photo.cshtml`.)
- **Verify:** Image renders when the URL is opened.

### Step 1.6 — `GET /api/products/top?count=10`
- **Touch:** `ProductEndpoints.cs`.
- **Do:** Top-N list for the home page (default 10, clamp max).
- **Verify:** Returns ≤ N items.

### Step 1.7 — Generate TS types from OpenAPI
- **Touch:** `SampleStore.Web/package.json` (script), `SampleStore.Web/src/api/schema.ts` (generated).
- **Do:** Add `openapi-typescript` dev dep + `"gen:api"` script pointing at `/openapi/v1.json`.
- **Verify:** `npm run gen:api` produces `schema.ts`.

### Step 1.8 — Typed fetch client
- **Touch:** `SampleStore.Web/src/api/client.ts`.
- **Do:** `fetch` wrapper with `credentials:"include"`, base `/api`, JSON helpers, error normalization. (CSRF header added in 3.1.)
- **Verify:** Unit-call `getProducts()` from a temp dev page returns data.

### Step 1.9 — `GET /api/auth/me`
- **Touch:** `SampleStore.Api/Endpoints/AuthEndpoints.cs`, `SampleStore.Api/Dtos/Auth/CurrentUserDto.cs`.
- **Do:** From `HttpContext.User`: `{ isAuthenticated, email, fullName, roles[] }` (anonymous → `isAuthenticated:false`). Works today because the Razor login already sets the cookie.
- **Verify:** Returns correct data when logged in via existing Razor login.

---

# Phase 2 — Migrate public read pages (flip routes)

### Step 2.1 — SPA shell + router
- **Touch:** `src/App.tsx`, `src/routes.tsx`, `src/components/Layout.tsx`.
- **Do:** Add React Router; build a Layout (header/nav/footer) mirroring `_Layout.cshtml`. Routes empty for now.
- **Verify:** Layout renders in dev.

### Step 2.2 — Auth context + nav state
- **Touch:** `src/auth/AuthContext.tsx`, `src/components/Nav.tsx`.
- **Do:** Provider calls `/api/auth/me`; nav shows user/roles. Login/Logout links point to the **existing Razor** `/Identity/Account/Login` and `/Identity/Account/Logout` for now.
- **Verify:** Logged-in vs anonymous nav differ correctly.

### Step 2.3a — Build Home page (React)
- **Touch:** `src/pages/Home.tsx`, route `/`.
- **Do:** Render top-10 via `/api/products/top`. Do not delete Razor yet; preview via temp path.
- **Verify:** Component renders products in dev.

### Step 2.3b — Flip Home route
- **Touch:** delete `SampleStore.UI/Pages/Index.cshtml` + `.cshtml.cs`.
- **Do:** `/` now falls through to the SPA.
- **Verify:** `/` is served by React in the running Host.

### Step 2.4 — Products list page
- **Touch:** `src/pages/Products/List.tsx`, route `/Products`; delete `Pages/Products/Index.cshtml(.cs)`.
- **Do:** List from `/api/products`; thumbnails use `/api/products/{id}/photo`.
- **Verify:** `/Products` served by React; images load.

### Step 2.5 — Product details page
- **Touch:** `src/pages/Products/Details.tsx`, route `/Products/Details/{id}` (match existing route shape); delete `Pages/Products/Details.cshtml(.cs)`.
- **Verify:** Details render; 404 handled.

### Step 2.6 — Remove the Photo Razor page
- **Touch:** delete `Pages/Products/Photo.cshtml(.cs)`.
- **Do:** All image `src` already point at `/api/products/{id}/photo`.
- **Verify:** No broken images anywhere.

### Step 2.7 — About / Contact / Privacy
- **Touch:** `src/pages/{About,Contact,Privacy}.tsx` + routes; delete the three Razor pages.
- **Do:** Port static content to JSX.
- **Verify:** Each URL served by React.

---

# Phase 3 — Admin Products (write API + SPA CRUD)

### Step 3.1 — Antiforgery + JSON 401/403 for `/api`
- **Touch:** `Program.cs`, `AuthEndpoints.cs` (token endpoint), `src/api/client.ts`.
- **Do:** Add antiforgery; `GET /api/antiforgery/token` issues a token; client sends it as `X-CSRF-TOKEN` on unsafe requests. Configure cookie events / endpoint filters so `/api/*` returns **401/403 JSON** instead of redirecting to the login path.
- **Verify:** Unauthenticated `POST /api/...` → 401 JSON (no redirect); GET token works.

### Step 3.2 — Create DTO + `POST /api/products`
- **Touch:** `Dtos/Products/CreateProductRequest.cs`, `ProductEndpoints.cs`.
- **Do:** `[Authorize(Roles=Admin)]`; accept multipart (fields + optional photo `IFormFile`); validate; create via `IUnitOfWork`; return `201` + details DTO.
- **Verify:** As admin (via Razor login) create succeeds; as non-admin → 403.

### Step 3.3 — Update DTO + `PUT /api/products/{id}`
- **Touch:** `Dtos/Products/UpdateProductRequest.cs`, `ProductEndpoints.cs`.
- **Do:** Admin-only update incl. photo replace; map `ConcurrencyException` → `409`.
- **Verify:** Update works; stale `Timestamp` → 409.

### Step 3.4 — `DELETE /api/products/{id}`
- **Touch:** `ProductEndpoints.cs`.
- **Do:** Admin-only delete; `404` if missing; `204` on success.
- **Verify:** Delete works; non-admin → 403.

### Step 3.5 — Admin route guard (SPA)
- **Touch:** `src/auth/RequireRole.tsx`.
- **Do:** Wrapper that redirects to login / Access Denied unless `roles` includes `Admin`.
- **Verify:** Non-admin can't reach guarded routes.

### Step 3.6 — Create product page
- **Touch:** `src/pages/Products/Create.tsx` + guarded route; delete `Pages/Products/Create.cshtml(.cs)`.
- **Verify:** Admin creates a product end-to-end.

### Step 3.7 — Edit product page
- **Touch:** `src/pages/Products/Edit.tsx` + guarded route; delete `Pages/Products/Edit.cshtml(.cs)`.
- **Do:** Prefill, photo replace, surface 409 concurrency message.
- **Verify:** Edit + concurrency path work.

### Step 3.8 — Delete product page
- **Touch:** `src/pages/Products/Delete.tsx` + guarded route; delete `Pages/Products/Delete.cshtml(.cs)`.
- **Verify:** Confirm + delete works.

### Step 3.9 — Admin affordances in product UI
- **Touch:** `src/pages/Products/*`.
- **Do:** Show Create/Edit/Delete links only when `Admin`.
- **Verify:** Links visible to admin only.

---

# Phase 4 — Migrate Identity (last; many small steps)

> API first, then flip each Identity Razor page to React using the **same URL** (e.g. `/Identity/Account/Login`) so cookie `LoginPath`, email links, and OAuth redirects keep working. OAuth stays as server-side challenge/callback redirects.

### Auth API

### Step 4.1 — `POST /api/auth/login`
- **Touch:** `AuthEndpoints.cs`, `Dtos/Auth/LoginRequest.cs`.
- **Do:** Use `ISignInManager`; return a result discriminating success / needs-2FA / locked-out / email-unconfirmed / invalid.
- **Verify:** Each branch returns the right status/payload.

### Step 4.2 — `POST /api/auth/logout`
- **Touch:** `AuthEndpoints.cs`.
- **Verify:** Cookie cleared; `/api/auth/me` → anonymous.

### Step 4.3 — `POST /api/auth/register`
- **Touch:** `AuthEndpoints.cs`, `Dtos/Auth/RegisterRequest.cs`.
- **Do:** Create user, send confirmation email (existing SendGrid sender).
- **Verify:** User created; email sent (or logged in dev).

### Step 4.4 — Confirm email + resend
- **Touch:** `AuthEndpoints.cs`.
- **Do:** `GET /api/auth/confirm-email?userId&code`; optional resend endpoint.
- **Verify:** Confirmation flips `EmailConfirmed`.

### Step 4.5 — Forgot / reset password
- **Touch:** `AuthEndpoints.cs`, `Dtos/Auth/{ForgotPasswordRequest,ResetPasswordRequest}.cs`.
- **Verify:** Reset token flow works against a test user.

### Step 4.6 — 2FA login endpoints
- **Touch:** `AuthEndpoints.cs`.
- **Do:** `POST /api/auth/login-2fa`, `POST /api/auth/login-recovery-code`.
- **Verify:** 2FA-enabled account can complete login.

### Step 4.7 — External login (OAuth) endpoints
- **Touch:** `AuthEndpoints.cs`.
- **Do:** `GET /api/auth/external/{provider}` issues a challenge; callback signs in / links / pre-fills registration, then **redirects** to the SPA. Keep existing Facebook/Google/Microsoft config + callback paths.
- **Verify:** At least one provider completes a round-trip in dev.

### Identity SPA pages (build component → flip same-URL route → delete Razor page)

### Step 4.8 — Login page → delete Razor `Account/Login`
### Step 4.9 — Register page → delete `Account/Register`
### Step 4.10 — Logout action/page → delete `Account/Logout`
### Step 4.11 — Forgot password + confirmation → delete those two Razor pages
### Step 4.12 — Reset password + confirmation → delete those two
### Step 4.13 — Confirm email page → delete `Account/ConfirmEmail`
### Step 4.14 — Login-with-2fa + recovery-code pages → delete those two
### Step 4.15 — Access Denied + Lockout pages → delete those two
### Step 4.16 — External login confirmation page → delete `Account/ExternalLogin`

*(Each 4.8–4.16 step: add `src/pages/Identity/...tsx` at the matching URL, wire to the Phase-4 API, verify the flow, then delete the corresponding `.cshtml(.cs)`. Update `Program.cs` `AllowAnonymousToAreaFolder` only as needed; cookie `LoginPath` stays `/Identity/Account/Login`.)*

### Account management (`/Identity/Account/Manage/*`)

### Step 4.17 — Profile API + page
- **Touch:** `Endpoints/AccountEndpoints.cs` (`GET/PUT /api/account/profile`), `src/pages/Account/Profile.tsx`; delete `Manage/Index`.
### Step 4.18 — Change password (API + page) → delete `Manage/ChangePassword`
### Step 4.19 — Set password (OAuth-only accounts) → delete `Manage/SetPassword`
### Step 4.20 — 2FA management: enable authenticator (+QR), recovery codes, disable, reset → delete the four `Manage` 2FA pages
### Step 4.21 — External logins management → delete `Manage/ExternalLogins`
### Step 4.22 — Personal data: view / download / delete (GDPR) → delete the three `Manage` personal-data pages

*(Each: add API endpoint(s) in `AccountEndpoints.cs` + the React page at the same URL, verify, then delete the Razor page. Split any step that grows — e.g. 2FA — into per-page sub-steps.)*

---

# Phase 5 — Cutover & cleanup

### Step 5.1 — Confirm no Razor UI pages remain
- **Do:** Inventory `SampleStore.UI/Pages` and `Areas`; only an `Error` page (if any) may remain.
- **Verify:** Every app URL is served by the SPA.

### Step 5.2 — Remove Razor Pages from the pipeline
- **Touch:** `Program.cs`.
- **Do:** Drop `AddRazorPages`, `MapRazorPages`, and the `AuthorizeFolder("/")` / `AllowAnonymousToAreaFolder` conventions. Keep API + static + `MapFallbackToFile`. Move any still-needed page-level auth to API authorization.
- **Verify:** App runs as a pure SPA + API; protected APIs still enforce auth.

### Step 5.3 — Retire `SampleStore.UI` Razor assets
- **Touch:** `SampleStore.UI` (cshtml, partials, `_Layout`, ViewModels), `Host.csproj`, `.slnx`.
- **Do:** Delete now-dead Razor files/ViewModels (DTOs replaced them). Remove the UI project from the solution if it's empty, or repurpose it. Drop the Razor SDK ref if unused.
- **Verify:** `dotnet build SampleStore.slnx` clean.

### Step 5.4 — Remove jQuery / Bootstrap 3 / unobtrusive-validation
- **Touch:** `wwwroot/lib/*`, old `site.js`/`site.css`.
- **Do:** Delete legacy client libs now that React owns the UI.
- **Verify:** No 404s; SPA styling intact.

### Step 5.5 — Production build integration
- **Touch:** `SampleStore.Host.csproj` (MSBuild target), publish config.
- **Do:** On `dotnet publish`, run `npm ci && npm run build` in `SampleStore.Web` and copy `dist/` into the Host's static web root.
- **Verify:** `dotnet publish` yields a self-contained, working SPA+API deployment.

### Step 5.6 — Production hardening
- **Touch:** `Program.cs`, SPA error boundary, `src/pages/NotFound.tsx`.
- **Do:** Cache-control/immutable headers for hashed assets; ensure fallback excludes `/api`; SPA 404 + error boundary; HSTS/HTTPS unchanged.
- **Verify:** Caching headers correct; deep-link refresh works; unknown route shows SPA 404.

### Step 5.7 — Docs
- **Touch:** `CLAUDE.md`, `README`.
- **Do:** Document the new architecture, dev/prod commands, and the API/SPA layout.
- **Verify:** A new dev can build and run from the docs alone.

---

# Phase 6 — Tests & CI (optional, recommended)

### Step 6.1 — API integration tests
- `WebApplicationFactory`-based tests for product endpoints (auth, CRUD, 404/409).

### Step 6.2 — React unit/component tests
- Vitest + Testing Library for key pages/components and the auth context.

### Step 6.3 — E2E smoke
- Playwright: login → browse products → admin create/edit/delete.

### Step 6.4 — CI pipeline
- Build/test both .NET and the SPA; run `gen:api` drift check; produce a publish artifact.

---

## 5. Risks & mitigations

- **Auth redirect vs JSON (3.1):** get the `/api/*` 401/403-JSON behavior right before building any SPA write flow, or forms will mysteriously "redirect."
- **Antiforgery/CSRF:** centralize the token in `client.ts` so every unsafe call carries `X-CSRF-TOKEN`.
- **OAuth round-trips (4.7):** keep server-side challenge/callback paths identical; only the final landing redirect changes (to a SPA route).
- **Route shape mismatches:** mirror existing Razor route templates exactly when flipping (e.g. `/Products/Details/{id}`), so bookmarks/links survive.
- **Optimistic concurrency:** preserve `Timestamp`/`ConcurrencyException` → `409` semantics in the API and surface them in edit forms.
- **Rollback:** every step is one commit; `git revert` restores the prior working state (deleted Razor page returns, fallback stops catching that route).

## 6. Definition of done

- No Razor Pages remain; all routes served by the React SPA.
- All UI talks to Minimal APIs over the existing cookie session with CSRF protection.
- `dotnet publish` builds and bundles the SPA automatically.
- Legacy client libraries (jQuery, Bootstrap 3, unobtrusive validation) removed.
- Docs updated; tests/CI green.
