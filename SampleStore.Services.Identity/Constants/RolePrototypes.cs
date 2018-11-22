namespace SampleStore.Services.Identity.Constants
{
    using System.Collections.Generic;

    using SampleStore.Data.Entities.Identity;

    /// <summary>
    /// Class encapsulating role prototypes.
    /// </summary>
    public static class RolePrototypes
    {
        #region Fields

        /// <summary>
        /// The admin
        /// </summary>
        public static readonly Role Admin = new Role
        {
            Name = "Admin"
        };

        /// <summary>
        /// The client
        /// </summary>
        public static readonly Role Client = new Role
        {
            Name = "Client"
        };

        /// <summary>
        /// The super admin
        /// </summary>
        public static readonly Role SuperAdmin = new Role
        {
            Name = "SuperAdmin"
        };

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the roles.
        /// </summary>
        public static IEnumerable<Role> Roles
        {
            get
            {
                yield return SuperAdmin;
                yield return Admin;
                yield return Client;
            }
        }

        #endregion Properties
    }
}