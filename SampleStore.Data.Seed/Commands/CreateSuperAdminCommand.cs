namespace SampleStore.Data.Seed.Commands
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using SampleStore.Common.Commands;
    using SampleStore.Common.Extensions;
    using SampleStore.Common.Services;
    using SampleStore.Data.Entities.Identity;
    using SampleStore.Data.Seed.Extensions;

    /// <summary>
    /// Class encapsulating create super admin command.
    /// </summary>
    /// <seealso cref="ICreateSuperAdminCommandFactory" />
    public class CreateSuperAdminCommand : ICommand<User>
    {
        #region Fields

        /// <summary>
        /// The date time service
        /// </summary>
        private readonly IDateTime _dateTimeService;

        /// <summary>
        /// The password
        /// </summary>
        private readonly string _password;

        /// <summary>
        /// The prototype
        /// </summary>
        private readonly User _prototype;

        /// <summary>
        /// The user manager
        /// </summary>
        private readonly UserManager<User> _userManager;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSuperAdminCommand" /> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="dateTimeService">The date time service.</param>
        /// <param name="prototype">The prototype.</param>
        /// <param name="password">The password.</param>
        public CreateSuperAdminCommand(UserManager<User> userManager, IDateTime dateTimeService, User prototype, string password)
        {
            _userManager = userManager.ThrowIfArgumentIsNull(nameof(userManager));
            _dateTimeService = dateTimeService.ThrowIfArgumentIsNull(nameof(dateTimeService));
            _prototype = prototype.ThrowIfArgumentIsNull(nameof(prototype));
            _password = password.ThrowIfArgumentIsNull(nameof(password));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates the super admin.
        /// </summary>
        /// <returns>
        /// The super admin user.
        /// </returns>
        public async Task<User> Do()
        {
            var superAdmin = new User();
            _prototype.CopyTo(superAdmin);
            superAdmin.DateOfBirth = _dateTimeService.UtcNow.Date;
            superAdmin.EmailConfirmed = true;

            var userResult = await _userManager.CreateAsync(superAdmin, _password);
            userResult.ThrowIfFailed(() => $"Failed to create user {superAdmin.Email}");

            return superAdmin;
        }

        #endregion Methods
    }
}