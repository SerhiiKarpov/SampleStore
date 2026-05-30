using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using SampleStore.Services.Identity;
using SampleStore.UI.Pages;

namespace SampleStore.UI.Areas.Identity.Pages.Account.Manage;

public class DownloadPersonalDataModel(
    IUserManager userManager,
    ILogger<DownloadPersonalDataModel> logger) : PageModelBase
{
    private readonly ILogger<DownloadPersonalDataModel> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IUserManager _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

    public override string Title => "Download Your Data";

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