using Microsoft.AspNetCore.Authorization;

namespace SampleStore.UI.Pages;

[AllowAnonymous]
public class ContactModel : PageModelBase
{
    public string? Message { get; set; }

    public override string Title => "Contact";

    public void OnGet()
    {
        Message = "Your contact page.";
    }
}