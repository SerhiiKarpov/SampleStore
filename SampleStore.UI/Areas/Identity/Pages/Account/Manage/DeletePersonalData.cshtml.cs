using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using SampleStore.Services.Identity;
using SampleStore.UI.Pages;
using SampleStore.UI.ViewModels.Identity;

namespace SampleStore.UI.Areas.Identity.Pages.Account.Manage;

public class DeletePersonalDataModel(
    IUserManager userManager,
    ISignInManager signInManager,
    ILogger<DeletePersonalDataModel> logger) : PageModelBase
{
    private readonly ILogger<DeletePersonalDataModel> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly ISignInManager _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    private readonly IUserManager _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

    [BindProperty]
    public DeletePersonalDataViewModel? Input { get; set; }

    public bool RequirePassword { get; set; }

    public override string Title => "Delete Personal Data";

    public async Task<IActionResult> OnGet()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        RequirePassword = await _userManager.HasPasswordAsync(user);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        RequirePassword = await _userManager.HasPasswordAsync(user);
        if (RequirePassword
            && !await _userManager.CheckPasswordAsync(user, Input?.Password ?? string.Empty))
        {
            ModelState.AddModelError(string.Empty, "Password not correct.");
            return Page();
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Unexpected error occurred deleteing user with ID '{user.Id}'.");
        }

        await _signInManager.SignOutAsync();

        _logger.LogInformation("User with ID '{UserId}' deleted themselves.", user.Id);

        return Redirect("~/");
    }
}