namespace SampleStore.Data.Entities.Identity
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class encapsulating user login.
    /// </summary>
    /// <seealso cref="Entity" />
    public class UserLogin : Entity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the login provider.
        /// </summary>
        /// <value>
        /// The login provider.
        /// </value>
        [Required]
        [StringLength(50)]
        public string LoginProvider
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the display name of the provider.
        /// </summary>
        /// <value>
        /// The display name of the provider.
        /// </value>
        [Required]
        [StringLength(100)]
        public string ProviderDisplayName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the provider key.
        /// </summary>
        /// <value>
        /// The provider key.
        /// </value>
        [Required]
        [StringLength(500)]
        public string ProviderKey
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public Guid UserId
        {
            get; set;
        }

        #endregion Properties
    }
}