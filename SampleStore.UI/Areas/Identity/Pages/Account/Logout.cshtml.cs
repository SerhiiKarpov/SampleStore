using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using SampleStore.Services.Identity;
using SampleStore.UI.Pages;

namespace SampleStore.UI.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LogoutModel(ISignInManager signInManager, ILogger<LogoutModel> logger) : PageModelBase
{
    private readonly ILogger<LogoutModel> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly ISignInManager _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));

    public override string Title => "Log out";

    public async Task<IActionResult> OnPost(string? returnUrl = null)
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");
        if (returnUrl != null)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            return Page();
        }
    }
}