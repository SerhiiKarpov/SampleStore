using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using SampleStore.Common.Extensions;
using SampleStore.Data.Entities.Identity;
using SampleStore.UI.Pages;

namespace SampleStore.UI.Areas.Identity.Pages.Account.Manage;

public class ResetAuthenticatorModel : PageModelBase
{
    private readonly SignInManager<User> _signInManager;

    private ILogger<ResetAuthenticatorModel> _logger;

    UserManager<User> _userManager;

    public ResetAuthenticatorModel(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<ResetAuthenticatorModel> logger)
    {
        _userManager = userManager.ThrowIfArgumentIsNull(nameof(userManager));
        _signInManager = signInManager.ThrowIfArgumentIsNull(nameof(signInManager));
        _logger = logger.ThrowIfArgumentIsNull(nameof(logger));
    }

    [TempData]
    public string? StatusMessage { get; set; }

    public override string Title
    {
        get
        {
            return "Reset authenticator key";
        }
    }

    public async Task<IActionResult> OnGet()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        await _userManager.SetTwoFactorEnabledAsync(user, false);
        await _userManager.ResetAuthenticatorKeyAsync(user);
        _logger.LogInformation("User with ID '{UserId}' has reset their authentication app key.", user.Id);

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Your authenticator app key has been reset, you will need to configure your authenticator app using the new key.";

        return RedirectToPage("./EnableAuthenticator");
    }
}