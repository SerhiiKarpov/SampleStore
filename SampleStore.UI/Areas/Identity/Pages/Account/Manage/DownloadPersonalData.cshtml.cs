using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Text.Json;

using SampleStore.Common.Extensions;
using SampleStore.Data.Entities.Identity;
using SampleStore.UI.Pages;

namespace SampleStore.UI.Areas.Identity.Pages.Account.Manage;

public class DownloadPersonalDataModel : PageModelBase
{
    private readonly ILogger<DownloadPersonalDataModel> _logger;

    private readonly UserManager<User> _userManager;

    public DownloadPersonalDataModel(
        UserManager<User> userManager,
        ILogger<DownloadPersonalDataModel> logger)
    {
        _userManager = userManager.ThrowIfArgumentIsNull(nameof(userManager));
        _logger = logger.ThrowIfArgumentIsNull(nameof(logger));
    }

    public override string Title
    {
        get
        {
            return "Download Your Data";
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        _logger.LogInformation("User with ID '{UserId}' asked for their personal data.", _userManager.GetUserId(User));

        // Only include personal data for download
        var personalData = new Dictionary<string, string>
        {
            { "Id", user.Id.ToString() },
            { "FullName", user.FullName },
            { "DateOfBirth", user.DateOfBirth.ToString() },
            { "Email", user.Email },
            { "PhoneNumber", user.PhoneNumber ?? string.Empty }
        };

        Response.Headers["Content-Disposition"] = "attachment; filename=PersonalData.json";
        return new FileContentResult(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(personalData)), "text/json");
    }
}