using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SampleStore.Common.Extensions;
using SampleStore.Data.EF.Extensions;
using SampleStore.Data.Seed.Extensions;
using SampleStore.Host.Configuration;
using SampleStore.Host.Extensions;
using SampleStore.Services.Email.SendGrid.Extensions;
using SampleStore.Services.Identity.Extensions;
using SampleStore.UI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCommonServices();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = _ => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddEntityFrameworkDataAccess(builder.Configuration.GetConnectionString("DefaultConnection")!);
builder.Services.AddDatabaseSeeder();

builder.Services.AddCustomizedIdentity();

builder.Services.AddAuthentication()
    .AddFacebook(options =>
    {
        var elo = builder.Configuration.GetSection("Authentication:Facebook").Get<ExternalLoginOptions>()!;
        options.AppId = elo.ClientId;
        options.AppSecret = elo.ClientSecret;
        options.CallbackPath = elo.CallbackPath;
        options.Fields.Add("first_name", "last_name", "email", "birthday");
        options.Scope.Add("public_profile", "email", "user_birthday");
    })
    .AddGoogle(options =>
    {
        var elo = builder.Configuration.GetSection("Authentication:Google").Get<ExternalLoginOptions>()!;
        options.ClientId = elo.ClientId;
        options.ClientSecret = elo.ClientSecret;
        options.CallbackPath = elo.CallbackPath;
        options.ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, "birthday");
    })
    .AddMicrosoftAccount(options =>
    {
        var elo = builder.Configuration.GetSection("Authentication:Microsoft").Get<ExternalLoginOptions>()!;
        options.ClientId = elo.ClientId;
        options.ClientSecret = elo.ClientSecret;
        options.CallbackPath = elo.CallbackPath;
    });

builder.Services.AddSendGridEmailSender(options =>
{
    options.SenderEmail = builder.Configuration["EmailSender:SenderEmail"];
    options.SendGridUser = builder.Configuration["EmailSender:SendGrid:User"];
    options.SendGridKey = builder.Configuration["EmailSender:SendGrid:Key"];
});

builder.Services.Configure<IdentityOptions>(builder.Configuration.GetSection("Identity"));
builder.Services.Configure<CookieAuthenticationOptions>(
    IdentityConstants.ApplicationScheme,
    builder.Configuration.GetSection("CookieAuthentication"));

builder.Services.ConfigureUI();

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");
    options.Conventions.AllowAnonymousToAreaFolder("Identity", "/Account");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.EnsureSeeded();
app.Run();
