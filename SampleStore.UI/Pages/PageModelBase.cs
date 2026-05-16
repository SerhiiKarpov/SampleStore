
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SampleStore.UI.Pages;
/// <summary>
/// Class encapsulating page model base.
/// </summary>
/// <seealso cref="PageModel" />
public abstract class PageModelBase : PageModel
{
    #region Properties

    /// <summary>
    /// Gets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    public abstract string Title
    {
        get;
    }

    #endregion Properties
}