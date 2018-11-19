namespace SampleStore.UI.ViewModels.Identity
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class encapsulating login view model.
    /// </summary>
    public class LoginViewModel
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

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Required]
        [DataType(DataType.Password)]
        public string Password
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [remember me].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [remember me]; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Remember me?")]
        public bool RememberMe
        {
            get; set;
        }

        #endregion Properties
    }
}