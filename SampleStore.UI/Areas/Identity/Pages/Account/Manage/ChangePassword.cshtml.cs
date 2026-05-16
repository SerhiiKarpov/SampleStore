using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using SampleStore.Common.Extensions;
using SampleStore.Data.Entities.Identity;
using SampleStore.UI.Pages;
using SampleStore.UI.ViewModels.Identity;

namespace SampleStore.UI.Areas.Identity.Pages.Account.Manage;

public class ChangePasswordModel : PageModelBase
{
    private readonly ILogger<ChangePasswordModel> _logger;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public ChangePasswordModel(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<ChangePasswordModel> logger)
    {
        _userManager = userManager.ThrowIfArgumentIsNull(nameof(userManager));
        _signInManager = signInManager.ThrowIfArgumentIsNull(nameof(signInManager));
        _logger = logger.ThrowIfArgumentIsNull(nameof(logger));
    }

    [BindProperty]
    public ChangePasswordViewModel? Input { get; set; }

    [TempData]
    public string? StatusMessage { get; set; }

    public override string Title => "Change password";

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var hasPassword = await _userManager.HasPasswordAsync(user);
        return !hasPassword ? RedirectToPage("./SetPassword") : (IActionResult)Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input!.OldPassword, Input!.NewPassword);
        if (!changePasswordResult.Succeeded)
        {
            foreach (var error in changePasswordResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        await _signInManager.RefreshSignInAsync(user);
        _logger.LogInformation("User changed their password successfully.");
        StatusMessage = "Your password has been changed.";

        return RedirectToPage();
    }
}