namespace SampleStore.Data.Entities.Identity
{
    using System;

    /// <summary>
    /// Class encapsulating user role.
    /// </summary>
    /// <seealso cref="Entity" />
    public class UserRole : Entity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the role identifier.
        /// </summary>
        public Guid RoleId
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