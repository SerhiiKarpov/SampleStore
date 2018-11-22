namespace SampleStore.Data.Entities.Identity
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class encapsulating user token.
    /// </summary>
    /// <seealso cref="Entity" />
    public class UserToken : Entity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the login provider.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string LoginProvider
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name
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

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [StringLength(500)]
        public string Value
        {
            get; set;
        }

        #endregion Properties
    }
}