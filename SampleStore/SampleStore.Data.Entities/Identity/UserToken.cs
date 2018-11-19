namespace SampleStore.Data.Entities.Identity
{
    using System;

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
        public string LoginProvider
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
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
        public string Value
        {
            get; set;
        }

        #endregion Properties
    }
}