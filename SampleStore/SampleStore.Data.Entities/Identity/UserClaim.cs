namespace SampleStore.Data.Entities.Identity
{
    using System;

    /// <summary>
    /// Class encapsulating user claim.
    /// </summary>
    /// <seealso cref="Entity" />
    public class UserClaim : Entity
    {
        #region Properties

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