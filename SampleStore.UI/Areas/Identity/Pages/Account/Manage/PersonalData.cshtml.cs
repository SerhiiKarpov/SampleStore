using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using SampleStore.Common.Extensions;
using SampleStore.Data.Entities.Identity;
using SampleStore.UI.Pages;

namespace SampleStore.UI.Areas.Identity.Pages.Account.Manage;

public class PersonalDataModel : PageModelBase
{
    private readonly ILogger<PersonalDataModel> _logger;

    private readonly UserManager<User> _userManager;

    public PersonalDataModel(
        UserManager<User> userManager,
        ILogger<PersonalDataModel> logger)
    {
        _userManager = userManager.ThrowIfArgumentIsNull(nameof(userManager));
        _logger = logger.ThrowIfArgumentIsNull(nameof(logger));
    }

    public override string Title
    {
        get
        {
            return "Personal Data";
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
}