namespace SampleStore.Data.Entities.Identity
{
    using System;

    /// <summary>
    /// Class encapsulating user login.
    /// </summary>
    /// <seealso cref="Entity" />
    public class UserLogin : Entity
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