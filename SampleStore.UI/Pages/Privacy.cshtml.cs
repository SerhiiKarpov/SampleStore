namespace SampleStore.UI.Pages
{
    using Microsoft.AspNetCore.Authorization;

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
}