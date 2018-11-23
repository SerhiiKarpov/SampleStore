namespace SampleStore.Data.Seed.Tests.Helpers
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Moq;

    using SampleStore.Data.Entities.Identity;

    /// <summary>
    /// Class encapsulating user manager test helper.
    /// </summary>
    public static class UserManagerTestHelper
    {
        #region Methods

        /// <summary>
        /// Creates the user manager fake.
        /// </summary>
        /// <returns>
        /// The user manager fake.
        /// </returns>
        public static Mock<UserManager<User>> CreateUserManagerFake()
        {
            var fake = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<User>>(),
                Enumerable.Empty<IUserValidator<User>>(),
                Enumerable.Empty<IPasswordValidator<User>>(),
                Mock.Of<ILookupNormalizer>(),
                new IdentityErrorDescriber(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<User>>>());
            return fake;
        }

        #endregion Methods
    }
}