namespace SampleStore.Data.Entities.Identity
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class encapsulating application user.
    /// </summary>
    /// <seealso cref="Entity" />
    public class User : Entity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime DateOfBirth
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(100)]
        public string Email
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [email confirmed].
        /// </summary>
        public bool EmailConfirmed
        {
            get;set;
        }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string FullName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the password hash.
        /// </summary>
        [StringLength(100)]
        public string PasswordHash
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        [StringLength(50)]
        public string PhoneNumber
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [phone number confirmed].
        /// </summary>
        public bool PhoneNumberConfirmed
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [two factor enabled].
        /// </summary>
        public bool TwoFactorEnabled
        {
            get; set;
        }

        #endregion Properties
    }
}