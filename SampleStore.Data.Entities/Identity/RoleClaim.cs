namespace SampleStore.Data.Entities.Identity
{
    using System;

    /// <summary>
    /// Class encapsulating role claim.
    /// </summary>
    /// <seealso cref="Entity" />
    public class RoleClaim : Entity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the role identifier.
        /// </summary>
        public Guid RoleId
        {
            get; set;
        }

        #endregion Properties
    }
}