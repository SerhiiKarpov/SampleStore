using Microsoft.AspNetCore.Authorization;

namespace SampleStore.UI.Pages;

[AllowAnonymous]
public class PrivacyModel : PageModelBase
{
    public override string Title
    {
        get
        {
            return "Privacy Policy";
        }
    }
}