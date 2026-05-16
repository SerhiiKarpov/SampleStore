using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Bogus;

using Microsoft.AspNetCore.Identity;

using Moq;

using SampleStore.Common.Helpers;
using SampleStore.Common.Services;
using SampleStore.Data.Entities.Identity;
using SampleStore.Data.Seed.Commands;
using SampleStore.Data.Seed.Tests.Helpers;

using Xunit;

namespace SampleStore.Data.Seed.Tests.Commands;

public static class CreateSuperAdminCommand_Do_Should
{
    [Fact]
    public static async Task Delegate_To_UserManager()
    {
        // Arrange
        var userManagerMock = UserManagerTestHelper.CreateUserManagerFake();
        userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(Task.FromResult(IdentityResult.Success));

        var dateTimeServiceStub = Mock.Of<IDateTime>();

        var prototypeStub = new Faker<User>().Generate();
        var passwordStub = string.Empty;

        var command = new CreateSuperAdminCommand(userManagerMock.Object, dateTimeServiceStub, prototypeStub, passwordStub);

        // Act
        var superAdmin = await command.Do();

        // Assert
        userManagerMock.Verify(x => x.CreateAsync(superAdmin, passwordStub), Times.Once);
    }

    [Fact]
    public static async Task Set_All_Properties_From_Prototype_Except_Id_DateOfBirth_EmailConfirmed()
    {
        // Arrange
        var userManagerStub = UserManagerTestHelper.CreateUserManagerFake();
        userManagerStub.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));

        var dateTimeServiceStub = Mock.Of<IDateTime>();

        var prototypeMock = new Faker<User>().Generate();
        var passwordStub = string.Empty;

        var command = new CreateSuperAdminCommand(userManagerStub.Object, dateTimeServiceStub, prototypeMock, passwordStub);

        // Act
        var superAdmin = await command.Do();

        // Assert
        Assert.NotNull(superAdmin);

        var propertiesToIgnore = new[]
        {
            PropertyHelper.GetProperty((User user) => user.Id),
            PropertyHelper.GetProperty((User user) => user.DateOfBirth),
            PropertyHelper.GetProperty((User user) => user.EmailConfirmed)
        };
        var propertiesToVerify = typeof(User).GetProperties(BindingFlags.Instance | BindingFlags.Public).Except(propertiesToIgnore);
        foreach (var propertyToVerify in propertiesToVerify)
        {
            var expected = propertyToVerify.GetValue(prototypeMock);
            var actual = propertyToVerify.GetValue(superAdmin);
            Assert.Equal(expected, actual);
        }
    }

    [Fact]
    public static async Task Set_DateOfBirth_To_UtcNow_Date()
    {
        // Arrange
        var userManagerStub = UserManagerTestHelper.CreateUserManagerFake();
        userManagerStub.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));

        var utcNowStub = new DateTime(2000, 1, 1, 1, 1, 1);
        var expectedDateOfBirth = utcNowStub.Date;
        var dateTimeServiceStub = new Mock<IDateTime>();
        dateTimeServiceStub.Setup(dts => dts.UtcNow).Returns(utcNowStub);

        var prototypeStub = new User();
        var passwordStub = string.Empty;

        var command = new CreateSuperAdminCommand(userManagerStub.Object, dateTimeServiceStub.Object, prototypeStub, passwordStub);

        // Act
        var superAdmin = await command.Do();

        // Assert
        Assert.NotNull(superAdmin);
        Assert.Equal(expectedDateOfBirth, superAdmin.DateOfBirth);
    }

    [Fact]
    public static async Task Set_EmailConfirmed_To_True()
    {
        // Arrange
        var userManagerStub = UserManagerTestHelper.CreateUserManagerFake();
        userManagerStub.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));

        var dateTimeServiceStub = Mock.Of<IDateTime>();

        var prototype = new User { EmailConfirmed = false };
        var password = string.Empty;

        var command = new CreateSuperAdminCommand(userManagerStub.Object, dateTimeServiceStub, prototype, password);

        // Act
        var superAdmin = await command.Do();

        // Assert
        Assert.NotNull(superAdmin);
        Assert.True(superAdmin.EmailConfirmed);
    }

    [Fact]
    public static async Task Throw_If_UserManager_Failed_To_Create_User()
    {
        // Arrange
        var userManagerMock = UserManagerTestHelper.CreateUserManagerFake();
        userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(Task.FromResult(IdentityResult.Failed(new IdentityError())));

        var dateTimeServiceStub = Mock.Of<IDateTime>();

        var prototypeStub = new User();
        var passwordStub = string.Empty;

        var command = new CreateSuperAdminCommand(userManagerMock.Object, dateTimeServiceStub, prototypeStub, passwordStub);

        // Act
        Func<Task> act = () => command.Do();

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(act);
    }
}