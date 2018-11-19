namespace SampleStore.UI.Areas.Identity.Pages.Account
{
    using Microsoft.AspNetCore.Authorization;

    using SampleStore.UI.Pages;

    /// <summary>
    /// Class encapsulating forgot password confirmation.
    /// </summary>
    /// <seealso cref="PageModelBase" />
    [AllowAnonymous]
    public class ForgotPasswordConfirmation : PageModelBase
    {
        #region Properties

        /// <summary>
        /// Gets the title.
        /// </summary>
        public override string Title
        {
            get
            {
                return "Forgot password confirmation";
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