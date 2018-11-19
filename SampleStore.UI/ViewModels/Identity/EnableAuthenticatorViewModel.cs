namespace SampleStore.UI.ViewModels.Identity
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class encapsulating enable authenticator view model.
    /// </summary>
    public class EnableAuthenticatorViewModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Verification Code")]
        public string Code
        {
            get; set;
        }

        #endregion Properties
    }
}