using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using SampleStore.Services.Identity;
using SampleStore.Services.Identity.Constants;
using SampleStore.UI.Extensions;
using SampleStore.UI.Mapping;
using SampleStore.UI.Pages;
using SampleStore.UI.ViewModels.Identity;

namespace SampleStore.UI.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ExternalLoginModel(
    ISignInManager signInManager,
    IUserManager userManager,
    IEmailSender emailSender,
    ILogger<ExternalLoginModel> logger) : PageModelBase
{
    private readonly IEmailSender _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
    private readonly ILogger<ExternalLoginModel> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly ISignInManager _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    private readonly IUserManager _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

    [TempData]
    public string? ErrorMessage { get; set; }

    [BindProperty]
    public ExternalLoginViewModel Input { get; set; } = default!;

    public string? LoginProvider { get; set; }

    public string? ReturnUrl { get; set; }

    public override string Title => "Register";

    public IActionResult OnGetAsync() => RedirectToPage("./Login");

    public async Task<IActionResult> OnGetCallbackAsync(string? returnUrl = null, string? remoteError = null)
    {
        returnUrl ??= Url.Content("~/");
        if (remoteError != null)
        {
            ErrorMessage = $"Error from external provider: {remoteError}";
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info is null)
        {
            ErrorMessage = "Error loading external login information.";
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }

        // Sign in the user with this external login provider if the user already has a login.
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        if (result.Succeeded)
        {
            _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity?.Name, info.LoginProvider);
            return LocalRedirect(returnUrl);
        }

        if (result.IsLockedOut)
        {
            return RedirectToPage("./Lockout");
        }

        // If the user does not have an account, then ask the user to create an account.
        ReturnUrl = returnUrl;
        LoginProvider = info.LoginProvider;
        Input = info.ToExternalLoginViewModel();

        return Page();
    }

    public IActionResult OnPost(string provider, string? returnUrl = null)
    {
        // Request a redirect to the external login provider.
        var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return new ChallengeResult(provider, properties);
    }

    public async Task<IActionResult> OnPostConfirmationAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        // Get the information about the user from the external login provider
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info is null)
        {
            ErrorMessage = "Error loading external login information during confirmation.";
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }

        LoginProvider = info.LoginProvider;
        ReturnUrl = returnUrl;

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = IdentityResult.Success;
        var user = await _userManager.FindByEmailAsync(Input.Email);
        if (user is null)
        {
            user = Input.ToUser();
            user.EmailConfirmed = true;
            result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await _userManager.AddToRolesAsync(user, [Roles.Client]);
            }
        }

        if (result.Succeeded)
        {
            result = await _userManager.AddLoginAsync(user, info);
        }

        if (result.Succeeded)
        {
            result = await _userManager.AddClaimsAsync(user, info.Principal.Claims);
        }

        if (!result.Succeeded)
        {
            ModelState.AddModelErrors(result);
            return Page();
        }

        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
        return LocalRedirect(returnUrl);
    }
}