using Microsoft.AspNetCore.Authorization;

using SampleStore.UI.Pages;

namespace SampleStore.UI.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ResetPasswordConfirmationModel : PageModelBase
{
    public override string Title
    {
        get
        {
            return "Reset password confirmation";
        }
    }
}