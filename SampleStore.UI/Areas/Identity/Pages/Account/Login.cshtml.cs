using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using SampleStore.Services.Identity;
using SampleStore.UI.Pages;
using SampleStore.UI.ViewModels.Identity;

namespace SampleStore.UI.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LoginModel(ISignInManager signInManager, ILogger<LoginModel> logger) : PageModelBase
{
    private readonly ILogger<LoginModel> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly ISignInManager _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));

    [TempData]
    public string? ErrorMessage { get; set; }

    public IList<AuthenticationScheme>? ExternalLogins { get; set; }

    [BindProperty]
    public LoginViewModel Input { get; set; } = default!;

    public string? ReturnUrl { get; set; }

    public override string Title => "Log in";

    public async Task OnGetAsync(string? returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        returnUrl = returnUrl ?? Url.Content("~/");

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await _signInManager.PasswordSignInAsync(
            Input.Email,
            Input.Password,
            Input.RememberMe,
            lockoutOnFailure: true);
        if (result.Succeeded)
        {
            _logger.LogInformation("User logged in.");
            return LocalRedirect(returnUrl);
        }

        if (result.RequiresTwoFactor)
        {
            return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, Input.RememberMe });
        }

        if (result.IsLockedOut)
        {
            _logger.LogWarning("User account locked out.");
            return RedirectToPage("./Lockout");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}