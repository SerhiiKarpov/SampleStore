namespace SampleStore.Data.Seed.Tests.Commands
{
    using System;

    using Moq;

    using SampleStore.Common.Services;
    using SampleStore.Data.Entities.Identity;
    using SampleStore.Data.Seed.Commands;
    using SampleStore.Data.Seed.Tests.Helpers;

    using Xunit;

    /// <summary>
    /// Class encapsulating <see cref="CreateSuperAdminCommand"/> constructor unit-tests.
    /// </summary>
    public static class CreateSuperAdminCommand_Constructor_Should
    {
        #region Methods

        /// <summary>
        /// Throws if date time service is null.
        /// </summary>
        [Fact]
        public static void Throw_If_DateTimeService_Is_Null()
        {
            // Arrange
            var userManagerStub = UserManagerTestHelper.CreateUserManagerFake();
            var passwordStub = string.Empty;
            var prototypeStub = new User();

            // Act
            Action act = () => new CreateSuperAdminCommand(userManagerStub.Object, null, prototypeStub, passwordStub);

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        /// <summary>
        /// Throws if password is null.
        /// </summary>
        [Fact]
        public static void Throw_If_Password_Is_Null()
        {
            // Arrange
            var userManagerStub = UserManagerTestHelper.CreateUserManagerFake();
            var dateTimeServiceStub = Mock.Of<IDateTime>();
            var prototypeStub = new User();

            // Act
            Action act = () => new CreateSuperAdminCommand(userManagerStub.Object, dateTimeServiceStub, prototypeStub, null);

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        /// <summary>
        /// Throws if prototype is null.
        /// </summary>
        [Fact]
        public static void Throw_If_Prototype_Is_Null()
        {
            // Arrange
            var userManagerStub = UserManagerTestHelper.CreateUserManagerFake();
            var dateTimeServiceStub = Mock.Of<IDateTime>();
            var passwordStub = string.Empty;

            // Act
            Action act = () => new CreateSuperAdminCommand(userManagerStub.Object, dateTimeServiceStub, null, passwordStub);

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        /// <summary>
        /// Throws if user manager is null.
        /// </summary>
        [Fact]
        public static void Throw_If_UserManager_Is_Null()
        {
            // Arrange
            var dateTimeServiceStub = Mock.Of<IDateTime>();
            var passwordStub = string.Empty;
            var prototypeStub = new User();

            // Act
            Action act = () => new CreateSuperAdminCommand(null, dateTimeServiceStub, prototypeStub, passwordStub);

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        #endregion Methods
    }
}