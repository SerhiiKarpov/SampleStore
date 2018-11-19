namespace SampleStore.UI.ViewModels.Identity
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class encapsulating account view model.
    /// </summary>
    public class AccountViewModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        /// <value>
        /// The date of birth.
        /// </value>
        [Required]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth
        {
            get; set;
        }

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
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Full name")]
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber
        {
            get; set;
        }

        #endregion Properties
    }
}