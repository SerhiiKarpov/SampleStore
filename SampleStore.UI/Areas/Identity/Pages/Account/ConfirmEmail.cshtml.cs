using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SampleStore.Services.Identity;
using SampleStore.UI.Pages;

namespace SampleStore.UI.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ConfirmEmailModel(IUserManager userManager) : PageModelBase
{
    private readonly IUserManager _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

    public override string Title => "Confirm email";

    public async Task<IActionResult> OnGetAsync(string userId, string code)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
        {
            return RedirectToPage("/Index");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{userId}'.");
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Error confirming email for user with ID '{userId}':");
        }

        return Page();
    }
}