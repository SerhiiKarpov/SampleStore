namespace SampleStore.Data.Entities.Identity
{
    using System;

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
        public DateTime DateOfBirth
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
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
        public string FullName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the password hash.
        /// </summary>
        public string PasswordHash
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
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