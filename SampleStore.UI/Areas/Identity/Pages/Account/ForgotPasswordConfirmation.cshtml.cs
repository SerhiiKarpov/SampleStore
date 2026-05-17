using Microsoft.AspNetCore.Authorization;

using SampleStore.UI.Pages;

namespace SampleStore.UI.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ForgotPasswordConfirmation : PageModelBase
{
    public override string Title
    {
        get
        {
            return "Forgot password confirmation";
        }
    }
}