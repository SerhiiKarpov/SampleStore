# SampleStore.UI → React + TypeScript Migration Plan

Migrating the server-rendered **Razor Pages** UI (`SampleStore.UI`) to a **React + TypeScript single-page application**.

## Final decisions

| Decision | Choice |
|---|---|
| Scope | **Full SPA** — including the entire ASP.NET Identity surface (login, registration, email confirmation, password reset, 2FA/authenticator, recovery codes, external OAuth, account management) |
| Styling | **Tailwind CSS** (Bootstrap removed entirely) |
| Auth model | **A — cookie auth driven by JSON APIs.** Reuse `SignInManager`/`UserManager` + the custom `UserStore`/`RoleStore`; login issues the httpOnly auth cookie; the SPA sends it via `credentials:'include'`; CSRF token on unsafe verbs. No JWT/token lifecycle. |
| API style | **Minimal APIs** (grouped endpoint maps + policy-based authorization), not MVC controllers |

## The core problem

Razor page logic lives in `.cshtml.cs` page models that talk directly to `IUnitOfWork`/`IQueryMaterializer` and return rendered HTML. React needs **JSON APIs**. So this is "extract a backend API + build a React frontend against it," not "rewrite the views."

The **Identity rewrite is the bulk and the highest risk** of the project — we are reimplementing security-sensitive flows (2FA, OAuth, password reset, lockout) that were previously battle-tested scaffolding. Budget the majority of effort and testing on the auth API (Phase 1b) — it is the critical path. Products is comparatively a few days of mechanical work.

## Authentication model (A) — details

- Keep the existing `SignInManager`/`UserManager` and custom `UserStore`/`RoleStore`. Wrap their calls in minimal-API endpoints that return JSON / status codes instead of rendering Razor.
- Login issues the **httpOnly auth cookie** (XSS-safe). The SPA sends it automatically with `credentials:'include'`.
- **CSRF:** add `app.UseAntiforgery()`; issue a token (cookie + header); form-bound endpoints validate automatically, JSON `POST`s validate manually.
- Configure the Identity cookie events so `/api/*` returns **401/403 instead of redirecting** to a login page.

**External OAuth is redirect-based** (a SPA cannot do the provider handshake via `fetch`):
1. React links to `GET /api/auth/external/{provider}?returnUrl=/...`
2. Server issues `Results.Challenge` → browser leaves to the provider.
3. Provider redirects to the **server** callback; server runs the existing external-login logic (sign in, or detect "new user needs to confirm details").
4. Server redirects the browser back into the SPA: `/` (signed in) or `/account/external-confirm` (a React form pre-filled from OAuth claims, mirroring today's `ExternalLogin.cshtml`).

---

## Phase 0 — Decisions & scaffolding

- **Bundler:** Vite + React + TS (strict mode — matches the C# `Nullable=enable` discipline).
- **Routing:** React Router.
- **Server state:** TanStack Query.
- **Forms/validation:** React Hook Form + Zod (replaces jQuery unobtrusive validation across *all* forms, including auth).
- **Styling:** Tailwind CSS. Build a small primitives layer (`Button`, `Input`, `FormField`, `Card`, `Alert`, `Modal`, `Nav`) so the ~30 screens stay consistent — this matters a lot now that every Identity page is hand-styled.
- **Hosting:** Vite dev server + proxy to the ASP.NET host in dev; ASP.NET serves built static assets with a SPA fallback to `index.html` in prod.
- Scaffold `SampleStore.Web` (Vite) + configure Tailwind, ESLint/Prettier, Zod schemas folder.

---

## Phase 1 — Backend API (the real work)

Add a minimal-API surface to the host. **Reuse existing services** (`IUnitOfWork`, `IQueryMaterializer`, `UiMapper`) — only the transport changes.

### Minimal-API mechanics

- **Organization:** group endpoints with `MapGroup` and register each group via an `IEndpointRouteBuilder` extension method — `app.MapProductEndpoints()`, `app.MapAuthEndpoints()`, `app.MapAccountEndpoints()`. Mirrors the project convention that *each layer registers itself via an extension method*.
  ```csharp
  var products = app.MapGroup("/api/products");
  products.MapGet("/", GetProducts).AllowAnonymous();
  products.MapPost("/", CreateProduct).RequireAuthorization(Policies.Admin);
  ```
- **DI:** services are **handler parameters**, not constructor injection — `async (IUnitOfWork uow, IQueryMaterializer qm) => {...}`. `PageModelBase` disappears.
- **Authorization:** replace `[Authorize]` / `[Authorize(Roles = Roles.Admin)]` with:
  - a default `.RequireAuthorization()` (SPA equivalent of the old `AuthorizeFolder("/")`),
  - an **Admin policy** (`options.AddPolicy(Policies.Admin, p => p.RequireRole(Roles.Admin))`) applied via `.RequireAuthorization(Policies.Admin)`,
  - public endpoints opt out with `.AllowAnonymous()`.
- **Results:** `TypedResults.Ok(dto)`, `Results.NotFound()`, `Results.Conflict()` (for `ConcurrencyException` → 409), `Results.File(image, mimeType)` (photo), `Results.Challenge(props, [provider])` (external OAuth).
- **File upload:** `IFormFile` binds as a handler parameter for `POST/PUT /api/products` (multipart).
- **Validation:** no `ModelState` — validate request records with an endpoint filter (FluentValidation optional) or inline checks.
- **DTOs:** introduce `ProductDto`, `CreateProductRequest`, etc. — never serialize EF entities directly (avoids the `Photo.Image` byte blob leaking into JSON).
- **OpenAPI:** `.WithOpenApi()` / built-in OpenAPI document → generate TS client types.

### 1a. Products API
- `GET  /api/products` — replaces `Products/Index.OnGetAsync` → `ProductDto[]`
- `GET  /api/products/{id}` — replaces `Details.OnGetAsync`
- `POST /api/products` — replaces `Create.OnPostAsync` (Admin; multipart photo upload)
- `PUT  /api/products/{id}` — replaces `Edit.OnPostAsync` (Admin; preserve optimistic concurrency → 409)
- `DELETE /api/products/{id}` — replaces `Delete.OnPostAsync` (Admin)
- `GET  /api/products/{id}/photo` — replaces `Photo.cshtml.cs`; returns `Results.File(image, mimeType)` (React points `<img src>` at it)
- `GET  /api/home/top-products` — top-10 for the home page
- `GET  /api/me` — current user (name, roles) for nav rendering + Admin gating

### 1b. Auth & account API (new, large chunk)
Each maps to an existing Identity Razor page model; reuse `SignInManager`/`UserManager`, return JSON / status codes.

**Account / session:**
- `GET  /api/me` — current user + roles
- `POST /api/auth/register` — confirm-email flow (link points to a SPA route)
- `GET  /api/auth/confirm-email?userId=&code=`
- `POST /api/auth/login` — returns success / `requires-2fa` / `lockout` states
- `POST /api/auth/logout`
- `POST /api/auth/forgot-password`, `POST /api/auth/reset-password`
- `POST /api/auth/login-2fa`, `POST /api/auth/login-recovery-code`

**External login:**
- `GET  /api/auth/external/{provider}` (Challenge)
- `GET  /api/auth/external/callback` (server callback → redirect into SPA)
- `POST /api/auth/external/confirm`

**Account management (the `Manage/` area):**
- `GET/POST /api/account/profile` (email, name, DOB, phone)
- `POST /api/account/change-password`, `POST /api/account/set-password`
- `GET  /api/account/2fa` (status)
- `POST /api/account/enable-authenticator` (returns shared key + `otpauth://` URI for the QR code)
- `POST /api/account/disable-2fa`, `POST /api/account/reset-authenticator`, `POST /api/account/generate-recovery-codes`
- `GET/POST /api/account/external-logins` (list/add/remove linked providers)
- `GET  /api/account/personal-data/download`, `POST /api/account/delete`

**Notes:**
- QR codes: server returns the `otpauth://` URI; **React renders the QR** (e.g. `qrcode.react`).

---

## Phase 2 — React foundation

- Generate TS types from OpenAPI (Products DTOs + all Identity request/response shapes that are C# ViewModels today).
- API client: `fetch` wrapper, `credentials:'include'`, CSRF header injection, central 401→`/login` and 403→`/access-denied`.
- `AuthProvider` / `useCurrentUser()` (`GET /api/me`), `isAdmin`, `<RequireAuth>` and `<RequireAdmin>` route guards (replacing `AuthorizeFolder("/")` + role attributes).
- Tailwind primitives + layout components (ported from Razor):
  - `_Layout.cshtml` → `<AppLayout>` (navbar + footer)
  - `_LoginPartial.cshtml` → `<UserMenu>`
  - `_CookieConsentPartial.cshtml` → `<CookieConsentBanner>`
  - `ActivePageTagHelper` → active-link styling via React Router `NavLink`
  - `Manage/_ManageNav.cshtml` → `<AccountNav>` sidebar

---

## Phase 3 — Migrate screens (order = lowest risk first)

1. **Static** — About, Contact, Privacy.
2. **Home** — top-10 grid via TanStack Query.
3. **Products** — list + details (public), then Create/Edit/Delete (Admin, multipart upload, 409 handling).
4. **Auth core** — login, logout, register, email confirmation, forgot/reset password.
5. **External OAuth** — challenge → callback → external-confirm.
6. **2FA suite** — login-with-2fa, recovery-code login, enable authenticator (QR), disable, reset, generate recovery codes.
7. **Account management** — profile, change/set password, external logins, personal data download/delete.

Each step is independently shippable behind the SPA fallback.

---

## Phase 4 — Cutover & cleanup

- `Program.cs`: register the minimal-API endpoint groups + SPA static files/fallback; **remove `MapRazorPages()` and the `AuthorizeFolder`/`AllowAnonymousToAreaFolder` conventions** (route guards now live in React).
- End-to-end verification of **every** auth path — where the bugs hide (2FA, lockout, external-login-new-user, concurrency).
- **Delete the entire `SampleStore.UI` Razor surface**: `Pages/`, `Areas/Identity/`, tag helpers, partials, `wwwroot/lib` (jQuery, Bootstrap, unobtrusive validation). Replace C# ViewModels with API DTOs.

---

## Reality check on scope

- **Mechanical / low risk:** Products migration (~days).
- **Bulk / high risk:** the Identity rewrite — security-sensitive flows reimplemented from scaffolding. Treat the auth API (Phase 1b) as the critical path and over-invest in testing it.
