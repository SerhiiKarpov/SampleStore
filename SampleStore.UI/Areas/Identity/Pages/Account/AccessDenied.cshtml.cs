namespace SampleStore.UI.Areas.Identity.Pages.Account
{
    using SampleStore.UI.Pages;

    /// <summary>
    /// Class encapsulating access denied model.
    /// </summary>
    /// <seealso cref="PageModelBase" />
    public class AccessDeniedModel : PageModelBase
    {
        #region Properties

        /// <summary>
        /// Gets the title.
        /// </summary>
        public override string Title
        {
            get
            {
                return "Access denied";
            }
        }

        #endregion Properties
    }
}