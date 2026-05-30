using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using SampleStore.Services.Identity;
using SampleStore.UI.Pages;

namespace SampleStore.UI.Areas.Identity.Pages.Account.Manage;

public class PersonalDataModel(
    IUserManager userManager,
    ILogger<PersonalDataModel> logger) : PageModelBase
{
    private readonly ILogger<PersonalDataModel> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IUserManager _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

    public override string Title => "Personal Data";

    public async Task<IActionResult> OnGet()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        return Page();
    }
}