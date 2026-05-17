using System;

using NSubstitute;

using SampleStore.Common.Services;
using SampleStore.Data.Entities.Identity;
using SampleStore.Data.Seed.Commands;
using SampleStore.Data.Seed.Tests.Helpers;

using Xunit;

namespace SampleStore.Data.Seed.Tests.Commands.CreateSuperAdminCommandTests;

public static class ConstructorTests
{
    [Fact]
    public static void Throws_If_DateTimeService_Is_Null()
    {
        // Arrange
        var userManagerStub = UserManagerTestHelper.CreateUserManagerFake();
        var passwordStub = string.Empty;
        var prototypeStub = new User();

        // Act
        Action act = () => new CreateSuperAdminCommand(userManagerStub, null!, prototypeStub, passwordStub);

        // Assert
        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public static void Throws_If_Password_Is_Null()
    {
        // Arrange
        var userManagerStub = UserManagerTestHelper.CreateUserManagerFake();
        var dateTimeServiceStub = Substitute.For<IDateTime>();
        var prototypeStub = new User();

        // Act
        Action act = () => new CreateSuperAdminCommand(userManagerStub, dateTimeServiceStub, prototypeStub, null!);

        // Assert
        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public static void Throws_If_Prototype_Is_Null()
    {
        // Arrange
        var userManagerStub = UserManagerTestHelper.CreateUserManagerFake();
        var dateTimeServiceStub = Substitute.For<IDateTime>();
        var passwordStub = string.Empty;

        // Act
        Action act = () => new CreateSuperAdminCommand(userManagerStub, dateTimeServiceStub, null!, passwordStub);

        // Assert
        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public static void Throws_If_UserManager_Is_Null()
    {
        // Arrange
        var dateTimeServiceStub = Substitute.For<IDateTime>();
        var passwordStub = string.Empty;
        var prototypeStub = new User();

        // Act
        Action act = () => new CreateSuperAdminCommand(null!, dateTimeServiceStub, prototypeStub, passwordStub);

        // Assert
        Assert.Throws<ArgumentNullException>(act);
    }
}
