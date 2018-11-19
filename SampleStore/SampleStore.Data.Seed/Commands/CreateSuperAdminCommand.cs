namespace SampleStore.Data.Seed.Commands
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using SampleStore.Common;
    using SampleStore.Data.Entities.Identity;
    using SampleStore.Data.Seed.Extensions;

    /// <summary>
    /// Class encapsulating create super admin command.
    /// </summary>
    /// <seealso cref="ICreateSuperAdminCommand" />
    public class CreateSuperAdminCommand : ICreateSuperAdminCommand
    {
        #region Fields

        /// <summary>
        /// The user manager
        /// </summary>
        private readonly UserManager<User> _userManager;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSuperAdminCommand"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        public CreateSuperAdminCommand(UserManager<User> userManager)
        {
            _userManager = userManager.ThrowIfArgumentIsNull(nameof(userManager));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates the super admin.
        /// </summary>
        /// <param name="prototype">The prototype.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// The super admin user.
        /// </returns>
        public async Task<User> CreateSuperAdmin(User prototype, string password)
        {
            prototype.ThrowIfArgumentIsNull(nameof(prototype));
            password.ThrowIfArgumentIsNull(nameof(password));

            var superAdmin = new User();
            prototype.CopyTo(superAdmin);
            superAdmin.DateOfBirth = DateTime.UtcNow.Date;
            superAdmin.EmailConfirmed = true;

            var userResult = await _userManager.CreateAsync(superAdmin, password);
            userResult.ThrowIfFailed(() => $"Failed to create user {superAdmin.Email}");

            return superAdmin;
        }

        #endregion Methods
    }
}