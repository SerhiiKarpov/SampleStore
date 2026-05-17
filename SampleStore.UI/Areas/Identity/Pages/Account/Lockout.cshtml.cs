using Microsoft.AspNetCore.Authorization;

using SampleStore.UI.Pages;

namespace SampleStore.UI.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LockoutModel : PageModelBase
{
    public override string Title
    {
        get
        {
            return "Locked out";
        }
    }
}