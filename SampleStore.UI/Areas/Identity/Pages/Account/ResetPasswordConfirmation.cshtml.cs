namespace SampleStore.UI.Areas.Identity.Pages.Account
{
    using Microsoft.AspNetCore.Authorization;

    using SampleStore.UI.Pages;

    /// <summary>
    /// Class encapsulating reset password confirmation model.
    /// </summary>
    /// <seealso cref="PageModelBase" />
    [AllowAnonymous]
    public class ResetPasswordConfirmationModel : PageModelBase
    {
        #region Properties

        /// <summary>
        /// Gets the title.
        /// </summary>
        public override string Title
        {
            get
            {
                return "Reset password confirmation";
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