# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build the full solution
dotnet build SampleStore.slnx

# Run the application
dotnet run --project SampleStore.Host/SampleStore.Host.csproj

# Run all tests
dotnet test SampleStore.Data.Seed.Tests/SampleStore.Data.Seed.Tests.csproj

# Run a single test by name
dotnet test SampleStore.Data.Seed.Tests/SampleStore.Data.Seed.Tests.csproj --filter "FullyQualifiedName~TestName"

# Apply EF migrations
dotnet ef database update --project SampleStore.Data.EF --startup-project SampleStore.Host
```

## Architecture

ASP.NET Core web application using Razor Pages (not MVC), targeting .NET 10. The solution is split into 11 projects across distinct layers:

```
SampleStore.Host              → entry point, DI wiring (Program.cs, minimal hosting model)
SampleStore.UI                → Razor Pages, Identity Areas, ViewModels
SampleStore.Data              → abstractions: IUnitOfWork, IRepository<T>, IQueryMaterializer
SampleStore.Data.Entities     → EF POCO models (User, Role, Claims, Product, Photo)
SampleStore.Data.EF           → EF Core DbContext + IUnitOfWork/IRepository implementations
SampleStore.Data.Extensions   → query extension methods (Find, FindById, Update)
SampleStore.Data.Seed         → DatabaseSeeder + command pattern for initial data
SampleStore.Data.Seed.Tests   → xUnit tests (Bogus + Moq)
SampleStore.Services.Identity → custom ASP.NET Identity UserStore/RoleStore
SampleStore.Services.Email.SendGrid → IEmailSender via SendGrid
SampleStore.Common            → shared utilities and extensions
```

### Code Quality

`Directory.Build.props` applies globally to all projects:
- `Nullable=enable` — null-safety is enforced everywhere
- `TreatWarningsAsErrors=true` — all warnings are build errors

### Data Access

All data access goes through **Unit of Work + Repository**:
- `IUnitOfWork.GetRepository<T>()` returns `IRepository<T>` (exposes `IQueryable` + Add/Remove)
- `IQueryMaterializer` abstracts async query execution (ToList, FirstOrDefault, Count, Any) — registered as singleton
- Extension methods in `SampleStore.Data.Extensions` add `Find()`, `FindById()`, `Update()` on top of `IRepository<T>`
- Optimistic concurrency via `Timestamp` column; violations throw `ConcurrencyException`

### Identity

Custom `UserStore` and `RoleStore` implement all ASP.NET Identity interfaces and persist via `IUnitOfWork` (no EF Identity package). Both stores use **partial classes** split by concern: `UserStore.Password.cs`, `UserStore.Claims.cs`, `UserStore.Login.cs`, etc.

### Mapping

Mapping is done via plain static extension methods (no AutoMapper). Each layer has its own internal mapper class:
- `SampleStore.Services.Identity/Mapping/IdentityMapper.cs` — Identity entity ↔ ASP.NET Identity types
- `SampleStore.UI/Mapping/UiMapper.cs` — ViewModels ↔ Data entities

### Dependency Injection

Each layer registers itself via an extension method on `IServiceCollection`:
- `AddCommonServices()`, `AddEntityFrameworkDataAccess()`, `AddCustomizedIdentity()`, etc.
- `Program.cs` composes these — add new services by following this same pattern.

### Startup & Seeding

`Program.cs` calls `.EnsureSeeded()` before `.Run()`. `DatabaseSeeder` uses a factory-based command pattern: `ICreateRolesCommandFactory`, `ICreateSuperAdminCommandFactory`, `IAddUserToRolesCommandFactory`. The super-admin prototype (email, name) is read from `appsettings.json` under `SuperAdminPrototype`.

### Configuration Keys

`appsettings.json` top-level sections:
- `Identity` — password policy, lockout settings
- `CookieAuthentication` — cookie expiry and paths
- `Authentication` — OAuth callback paths (Facebook, Google, Microsoft)
- `EmailSender` — SendGrid credentials
- `SuperAdminPrototype` — default admin user seeded on first run
- `ConnectionStrings:DefaultConnection` — SQL Server connection string

### Global Authorization

All pages require authentication by default (`options.Conventions.AuthorizeFolder("/")`). Identity area pages are public via `AllowAnonymousToAreaFolder("Identity", "/Account")`.
