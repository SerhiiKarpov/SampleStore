using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SampleStore.UI.Pages;

public abstract class PageModelBase : PageModel
{
    public abstract string Title { get; }
}