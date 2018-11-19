namespace SampleStore.UI.ViewModels.Identity
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Class encapsulating login with recovery code view model.
    /// </summary>
    public class LoginWithRecoveryCodeViewModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the recovery code.
        /// </summary>
        /// <value>
        /// The recovery code.
        /// </value>
        [BindProperty]
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Recovery Code")]
        public string RecoveryCode
        {
            get; set;
        }

        #endregion Properties
    }
}