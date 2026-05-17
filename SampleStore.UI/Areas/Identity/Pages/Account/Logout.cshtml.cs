using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using SampleStore.Common.Extensions;
using SampleStore.Data.Entities.Identity;
using SampleStore.UI.Pages;

namespace SampleStore.UI.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LogoutModel : PageModelBase
{
    private readonly ILogger<LogoutModel> _logger;

    private readonly SignInManager<User> _signInManager;

    public LogoutModel(SignInManager<User> signInManager, ILogger<LogoutModel> logger)
    {
        _signInManager = signInManager.ThrowIfArgumentIsNull(nameof(signInManager));
        _logger = logger.ThrowIfArgumentIsNull(nameof(logger));
    }

    public override string Title
    {
        get
        {
            return "Log out";
        }
    }

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