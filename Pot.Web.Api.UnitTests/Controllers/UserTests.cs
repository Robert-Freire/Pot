namespace Pot.Web.Api.Unit.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Moq;

    using NUnit.Framework;

    using Pot.Data.BoundedContext.Pot;
    using Pot.Data.Model;
    using Pot.Web.Api.Controllers;
    using Pot.Web.Api.Model;

    /// <summary>
    /// The user.
    /// </summary>
    [TestFixture]
    public class UserTests
    {
        /// <summary>
        /// The get all users when there are two users then returns two users.
        /// </summary>
        [Test]
        public void GetAllUsers_ThereAreTwoUsers_ReturnsTwoUsers()
        {
            // Arrange
            var users = new DefaultSavedUsers().GetUsers();
            var userController = UserControllerMockFactory(users);

            // Action
            var result = userController.GetUsers().Result;

            // Assert
            result.Count().Should().Be(users.Count());
        }

//        /// <summary>
//        /// The get user when there are no users then returns not found.
//        /// </summary>
//        [Test]
//        public void GetUser_ThereAreNoUsers_ReturnsNotFound()
//        {
//            // Arrange
//            var users = new List<User>().AsQueryable();
//            var userController = UserControllerMockFactory(users);

//            // Action
//            var result = userController.Get(Guid.NewGuid()).Result;

//            // Assert
//            result.Should().BeOfType(typeof(System.Web.Http.Results.NotFoundResult));
//        }

//        /// <summary>
//        /// The get user when there are a user then returns user.
//        /// </summary>
//        [Test]
//        public void GetUser_ThereAreUser_ReturnUser()
//        {
//            var savedList = new DefaultSavedUsers();
//            var users = savedList.GetUsers();
//            var userController = UserControllerMockFactory(users);

//            // Action
//            var result = userController.Get(savedList.User1.IdUser).Result;

//            // Assert
//            result.ShouldBeEquivalent(savedList.User1);
//        }

//        /// <summary>
//        /// The update user if the userId and the userId are different returns bad request.
//        /// </summary>
//        [Test]
//        public void UpdateUser_ParametersWrong_ReturnsBadRequest()
//        {
//            // Arrange
//            var savedList = new DefaultSavedUsers();
//            var users = savedList.GetUsers();
//            var userController = UserControllerMockFactory(users);

//            // Action
//            var result = userController.Put(Guid.NewGuid(), new UserResource().MapFrom(savedList.User1)).Result;

//            // Assert
//            result.Should().BeOfType(typeof(System.Web.Http.Results.BadRequestResult));
//        }

//        /// <summary>
//        /// The update user if the user not exists then returns not found.
//        /// </summary>
//        [Test]
//        public void UpdateUser_UserNotExists_ReturnsNotFound()
//        {
//            // Arrange
//            var savedList = new DefaultSavedUsers();
//            var users = savedList.GetUsers();
//            var userController = UserControllerMockFactory(users);
//            var missingUser = new User { IdUser = Guid.NewGuid(), Name = "Missing user" };

//            // Action
//            var result = userController.Put(missingUser.IdUser, new UserResource().MapFrom(missingUser)).Result;

//            // Assert
//            result.Should().BeOfType(typeof(System.Web.Http.Results.NotFoundResult));
//        }

//        /// <summary>
//        /// The update user if the user not exists then returns not found.
//        /// </summary>
//        [Test]
//        public void UpdateUser_UserExists_ReturnsOk()
//        {
//            // Arrange
//            var savedList = new DefaultSavedUsers();
//            var users = savedList.GetUsers();
//            var userController = UserControllerMockFactory(users, savedList.User2);

//            var modifiedUser = new UserResource().MapFrom(savedList.User2);
//            modifiedUser.name = "modified name";

//            // Action
//            var result = userController.Put(modifiedUser.idUser, modifiedUser).Result;

//            // Assert
//            result.Should().BeOfType(typeof(System.Web.Http.Results.StatusCodeResult));
//        }

//        /// <summary>
//        /// The delete user if the user exists then returns ok.
//        /// </summary>
//        [Test]
//        public void DeleteUser_UserExists_ReturnsOk()
//        {
//            // Arrange
//            var savedList = new DefaultSavedUsers();
//            var users = savedList.GetUsers();
//            var userController = UserControllerMockFactory(users, savedList.User1);

//            // Action
//            var result = userController.Delete(savedList.User1.IdUser).Result;

//            // Assert
//            result.ShouldBeEquivalent(savedList.User1);
//        }

//        /// <summary>
//        /// The delete user if the user not exists then returns error.
//        /// </summary>
//        [Test]
//        public void DeleteUser_UserDoNotExists_ReturnsError()
//        {
//            // Arrange
//            var users = new List<User>().AsQueryable();
//            var userController = UserControllerMockFactory(users);

//            // Action
//            var result = userController.Delete(Guid.NewGuid()).Result;

//            // Assert
//            result.Should().BeOfType(typeof(System.Web.Http.Results.NotFoundResult));
//        }

        /// <summary>
        /// The user controller mock factory.
        /// </summary>
        /// <param name="users">
        /// The users.
        /// </param>
        /// <param name="userFound">
        /// The user found.
        /// </param>
        /// <returns>
        /// The <see cref="UsersController"/>.
        /// </returns>
        private static UsersController UserControllerMockFactory(IEnumerable<User> users, User userFound = null)
        {
            AutomapperConfig.Initialize();
            var userContextMock = new UserContextFake();
            userContextMock.SetupSets(users.AsQueryable());
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(cr => cr.FindAsync(It.IsAny<object>())).Returns(Task.FromResult(userFound));

            return new UsersController(new UserFactoryFake(userContextMock.Object));
        }

        /// <summary>
        /// The default saved users.
        /// </summary>
        private class DefaultSavedUsers
        {
            /// <summary>
            /// The user 1.
            /// </summary>
            private User user1;

            /// <summary>
            /// The user 2.
            /// </summary>
            private User user2;

            /// <summary>
            /// Gets the user 1.
            /// </summary>
            internal User User1
            {
                get
                {
                    return this.user1 ?? (this.user1 = new User { UserId = Guid.NewGuid(), Name = "Cliente 1" });
                }
            }

            /// <summary>
            /// Gets the user 2.
            /// </summary>
            internal User User2
            {
                get
                {
                    return this.user2 ?? (this.user2 = new User { UserId = Guid.NewGuid(), Name = "Cliente 2" });
                }
            }

            /// <summary>
            /// The get users list.
            /// </summary>
            /// <returns>
            /// The <see cref="IQueryable"/>.
            /// </returns>
            public IEnumerable<User> GetUsers()
            {
                return new List<User> { this.User1, this.User2 };
            }
        }
    }
}
