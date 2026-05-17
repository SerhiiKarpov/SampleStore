using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using SampleStore.Common.Extensions;
using SampleStore.Data.Entities.Identity;
using SampleStore.UI.Pages;
using SampleStore.UI.ViewModels.Identity;

namespace SampleStore.UI.Areas.Identity.Pages.Account.Manage;

public class SetPasswordModel : PageModelBase
{
    private readonly SignInManager<User> _signInManager;

    private readonly UserManager<User> _userManager;

    public SetPasswordModel(
        UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _userManager = userManager.ThrowIfArgumentIsNull(nameof(userManager));
        _signInManager = signInManager.ThrowIfArgumentIsNull(nameof(signInManager));
    }

    [BindProperty]
    public SetPasswordViewModel? Input { get; set; }

    [TempData]
    public string? StatusMessage { get; set; }

    public override string Title
    {
        get
        {
            return "Set password";
        }
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var hasPassword = await _userManager.HasPasswordAsync(user);

        if (hasPassword)
        {
            return RedirectToPage("./ChangePassword");
        }

        return Page();
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

        var addPasswordResult = await _userManager.AddPasswordAsync(user, Input!.NewPassword);
        if (!addPasswordResult.Succeeded)
        {
            foreach (var error in addPasswordResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Your password has been set.";

        return RedirectToPage();
    }
}