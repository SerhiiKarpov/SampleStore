
using Microsoft.AspNetCore.Authorization;

namespace SampleStore.UI.Pages;
/// <summary>
/// Class encapsulating privacy model.
/// </summary>
/// <seealso cref="PageModelBase" />
[AllowAnonymous]
public class PrivacyModel : PageModelBase
{
    #region Properties

    /// <summary>
    /// Gets the title.
    /// </summary>
    public override string Title
    {
        get
        {
            return "Privacy Policy";
        }
    }

    #endregion Properties
}