using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using Microsoft.Extensions.Options;

using NSubstitute;

using SampleStore.Data.Entities.Identity;
using SampleStore.Data.Seed.Configuration;
using SampleStore.Data.Seed.Extensions;
using SampleStore.Services.Identity;

using Xunit;

using Sut = SampleStore.Data.Seed.Commands.CreateSuperAdminCommand;

namespace SampleStore.Data.Seed.Tests.Commands.CreateSuperAdminCommandTests;

public static class DoTests
{
    private static readonly User _user =
        new()
        {
            Email = "test@example.com",
            EmailConfirmed = false,
            PhoneNumber = "+1234567890",
            PhoneNumberConfirmed = true,
            FullName = "John Doe",
            DateOfBirth = default,
            PasswordHash = "********",
            TwoFactorEnabled = true,
            RowVersion = [1, 2, 3, 4]
        };
    private const string Password = "Test123!";

    [Fact]
    public static async Task Creates_User_And_Adds_It_To_Roles()
    {
        // Arrange
        User? createdUser = null;
        User? userAddedToRoles = null;
        var userManager =
            CreateUserManager(
                create: (user, password) =>
                {
                    createdUser = user;
                    return user?.Id == _user.Id && password == Password
                        ? IdentityResult.Success
                        : IdentityResult.Failed();
                },
                addToRoles: (user, roles) =>
                {
                    userAddedToRoles = user;
                    return user?.Id == _user.Id && roles == Sut.Roles
                        ? IdentityResult.Success
                        : IdentityResult.Failed();
                });
        var sut = CreateSut(userManager);

        // Act
        await sut.Do();

        // Assert
        Received.InOrder(
            () =>
            {
                userManager.CreateAsync(Arg.Is<User>(x => x.Id == _user.Id), Password);
                userManager.AddToRolesAsync(Arg.Is<User>(x => x.Id == _user.Id), Sut.Roles);
            });

        Assert.NotNull(createdUser);
        Assert.Equal(10, _user.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Length);
        Assert.Equal(_user.Id, createdUser.Id);
        Assert.Equivalent(_user.RowVersion, createdUser.RowVersion, strict: true);
        Assert.Equal(_user.Email, createdUser.Email);
        Assert.True(createdUser.EmailConfirmed);
        Assert.Equal(_user.PhoneNumber, createdUser.PhoneNumber);
        Assert.Equal(_user.PhoneNumberConfirmed, createdUser.PhoneNumberConfirmed);
        Assert.Equal(_user.FullName, createdUser.FullName);
        Assert.Equal(DateTime.UtcNow.Date, createdUser.DateOfBirth);
        Assert.Equal(_user.PasswordHash, createdUser.PasswordHash);
        Assert.Equal(_user.TwoFactorEnabled, createdUser.TwoFactorEnabled);

        Assert.Same(createdUser, userAddedToRoles);
    }

    [Fact]
    public static async Task Throws_If_Failed_To_Create_User()
    {
        // Arrange
        var failure = CreateFailure();
        var userManager =
            CreateUserManager(
                create: (user, password) =>
                    user?.Id == _user.Id && password == Password
                        ? failure
                        : IdentityResult.Success);
        var sut = CreateSut(userManager);
        var expectedErrorMessage = failure.GetErrorMessage(() => Sut.GetCreateUserErrorMessage(_user.Email));

        // Act
        async Task act() => await sut.Do();

        // Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(act);
        Assert.Equal(expectedErrorMessage, exception.Message);
    }

    [Fact]
    public static async Task Throws_If_Faled_To_Add_User_To_Roles()
    {
        // Arrange
        var failure = CreateFailure();
        var userManager =
            CreateUserManager(
                addToRoles: (user, roles) =>
                    user?.Id == _user.Id && roles == Sut.Roles
                        ? failure
                        : IdentityResult.Success);
        var sut = CreateSut(userManager);
        var expectedErrorMessage = failure.GetErrorMessage(() => Sut.GetAddToRolesErrorMessage(_user.Email, Sut.Roles));

        // Act
        async Task act() => await sut.Do();

        // Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(act);
        Assert.Equal(expectedErrorMessage, exception.Message);       
    }

    private static IdentityResult CreateFailure()
    {
        const string ErrorCode1 = "420";
        const string ErrorDescription1 = "Test Error 1";
        const string ErrorCode2 = "69";
        const string ErrorDescription2 = "Test Error 2";
        var failure =
            IdentityResult.Failed(
                new() { Code = ErrorCode1, Description = ErrorDescription1 },
                new() { Code = ErrorCode2, Description = ErrorDescription2 });
        return failure;
    }

    private static SuperAdminOptions CreateOptions(User? prototype = null, string password = Password) =>
        new()
        {
            Prototype = prototype ?? _user,
            Password = password
        };

    private static IUserManager CreateUserManager(
        Func<User, string, IdentityResult>? create = null,
        Func<User, IEnumerable<string>, IdentityResult>? addToRoles = null)
    {
        create ??=
            (user, password) =>
                user?.Id == _user.Id && password == Password
                    ? IdentityResult.Success
                    : IdentityResult.Failed();
        addToRoles ??=
            (user, roles) =>
                user?.Id == _user.Id && roles == Sut.Roles
                    ? IdentityResult.Success
                    : IdentityResult.Failed();
        var stub = Substitute.For<IUserManager>();
        stub.CreateAsync(default!, default!)
            .ReturnsForAnyArgs(x => create(x.ArgAt<User>(0), x.ArgAt<string>(1)));
        stub.AddToRolesAsync(default!, default!)
            .ReturnsForAnyArgs(x => addToRoles(x.ArgAt<User>(0), x.ArgAt<IEnumerable<string>>(1)));
        return stub;
    }

    private static Sut CreateSut(
        IUserManager? userManager = null,
        SuperAdminOptions? options = null) =>
        new(
            userManager ?? CreateUserManager(),
            Options.Create(options ?? CreateOptions()));
}
