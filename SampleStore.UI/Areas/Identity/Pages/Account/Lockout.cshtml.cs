
using Microsoft.AspNetCore.Authorization;

using SampleStore.UI.Pages;

namespace SampleStore.UI.Areas.Identity.Pages.Account;
/// <summary>
/// Class encapsulating lockout model.
/// </summary>
/// <seealso cref="PageModelBase" />
[AllowAnonymous]
public class LockoutModel : PageModelBase
{
    #region Properties

    /// <summary>
    /// Gets the title.
    /// </summary>
    public override string Title
    {
        get
        {
            return "Locked out";
        }
    }

    #endregion Properties
}