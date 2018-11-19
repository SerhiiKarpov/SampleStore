namespace SampleStore.UI.ViewModels.Identity
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class encapsulating external login view model.
    /// </summary>
    public class ExternalLoginViewModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Required]
        [EmailAddress]
        public string Email
        {
            get; set;
        }

        #endregion Properties
    }
}