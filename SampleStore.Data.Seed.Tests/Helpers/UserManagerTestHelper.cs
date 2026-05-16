
using System;
using System.Linq;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using NSubstitute;

using SampleStore.Data.Entities.Identity;

namespace SampleStore.Data.Seed.Tests.Helpers;
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
    public static UserManager<User> CreateUserManagerFake()
    {
        return Substitute.For<UserManager<User>>(
            Substitute.For<IUserStore<User>>(),
            Substitute.For<IOptions<IdentityOptions>>(),
            Substitute.For<IPasswordHasher<User>>(),
            Enumerable.Empty<IUserValidator<User>>(),
            Enumerable.Empty<IPasswordValidator<User>>(),
            Substitute.For<ILookupNormalizer>(),
            new IdentityErrorDescriber(),
            Substitute.For<IServiceProvider>(),
            Substitute.For<ILogger<UserManager<User>>>());
    }

    #endregion Methods
}
