using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using SampleStore.Services.Identity;
using SampleStore.UI.Pages;

namespace SampleStore.UI.Areas.Identity.Pages.Account.Manage;

public class GenerateRecoveryCodesModel(
    IUserManager userManager,
    ILogger<GenerateRecoveryCodesModel> logger) : PageModelBase
{
    private readonly ILogger<GenerateRecoveryCodesModel> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IUserManager _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

    [TempData]
    public string[] RecoveryCodes { get; set; } = [];

    [TempData]
    public string? StatusMessage { get; set; }

    public override string Title => "Generate two-factor authentication (2FA) recovery codes";

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
        if (!isTwoFactorEnabled)
        {
            throw new InvalidOperationException($"Cannot generate recovery codes for user with ID '{user.Id}' because they do not have 2FA enabled.");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
        if (!isTwoFactorEnabled)
        {
            throw new InvalidOperationException($"Cannot generate recovery codes for user with ID '{user.Id}' as they do not have 2FA enabled.");
        }

        var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
        RecoveryCodes = recoveryCodes?.ToArray() ?? [];

        _logger.LogInformation("User with ID '{UserId}' has generated new 2FA recovery codes.", user.Id);
        StatusMessage = "You have generated new recovery codes.";
        return RedirectToPage("./ShowRecoveryCodes");
    }
}