using Microsoft.AspNetCore.Authorization;

namespace SampleStore.UI.Pages;

[AllowAnonymous]
public class AboutModel : PageModelBase
{
    public string Message { get; set; } = string.Empty;

    public override string Title => "About";

    public void OnGet()
    {
        Message = "Your application description page.";
    }
}