namespace SampleStore.UI.ViewModels.Identity
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class encapsulating delete personal data view model.
    /// </summary>
    public class DeletePersonalDataViewModel
    {
        #region Properties

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

        #endregion Properties
    }
}