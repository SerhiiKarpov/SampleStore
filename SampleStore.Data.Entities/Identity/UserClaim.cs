namespace SampleStore.Data.Entities.Identity
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class encapsulating user claim.
    /// </summary>
    /// <seealso cref="Entity" />
    public class UserClaim : Entity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [StringLength(100)]
        public string Type
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
        /// <value>
        /// The value.
        /// </value>
        [StringLength(200)]
        public string Value
        {
            get; set;
        }

        #endregion Properties
    }
}