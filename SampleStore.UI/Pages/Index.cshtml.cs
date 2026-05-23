using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SampleStore.UI.Pages;

[AllowAnonymous]
public class IndexModel : PageModelBase
{
    public override string Title => "Home page";
}