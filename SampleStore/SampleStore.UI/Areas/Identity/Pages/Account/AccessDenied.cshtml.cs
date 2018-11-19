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

        #region Methods

        /// <summary>
        /// Called when [get].
        /// </summary>
#pragma warning disable CA1822 // Mark members as static
        public void OnGet()
#pragma warning restore CA1822 // Mark members as static
        {
        }

        #endregion Methods
    }
}