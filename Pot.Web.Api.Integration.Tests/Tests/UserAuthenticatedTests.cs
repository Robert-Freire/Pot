// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserAuthenticatedTests.cs" company="Nova Language Services">
//   Copyright © Nova Language Services 2014
// </copyright>
// <summary>
//   The user OWIN authenticated tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Pot.Web.Api.Integration.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Newtonsoft.Json;

    using NUnit.Framework;

    using Pot.Data.Infraestructure;
    using Pot.Data.Model;
    using Pot.Data.SQLServer;
    using Pot.Web.Api.Controllers;
    using Pot.Web.Api.Integration.Tests.Utils;
    using Pot.Web.Api.Model;
    using Pot.Web.Api.Tests.Utils;

    /// <summary>
    /// The user OWIN authenticated tests.
    /// </summary>
    [TestFixture]
    public class UserAuthenticatedTests : BaseAuthenticatedTests
    {
        /// <summary>
        /// The authorization repository.
        /// </summary>
        private readonly IRepositoryAsync<User> userRepository;

        /// <summary>
        /// The uri.
        /// </summary>
        private string uri;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAuthenticatedTests"/> class.
        /// </summary>
        public UserAuthenticatedTests()
        {
            var authFactory = new UserFactory(new PotDbContext());
            this.userRepository = authFactory.UsersRepository;
        }

        /// <summary>
        /// Gets the uri.
        /// </summary>
        protected override Uri Uri
        {
            get
            {
                return new Uri(DefaultServerUri + this.uri, UriKind.RelativeOrAbsolute);
            }
        }

        /// <summary>
        /// The get all users if there are two users then returns two users.
        /// </summary>
        [Test]
        public void GetAllUsers_ThereAreAlmosTwoUsers_ReturnsTwoUsersOrMore()
        {
            this.uri = UsersController.UriBase;

            using (var response = this.GetAsync().Result)
            {
                // Assert
                response.Should().NotBeNull();
                var users = JsonConvert.DeserializeObject<List<UserResource>>(response.Content.ReadAsStringAsync().Result);
                users.Count.Should().BeGreaterOrEqualTo(2);
            }
        }

        /// <summary>
        /// The get user if there are no users then returns not found.
        /// </summary>
        [Test]
        public void GetUser_ThereAreNoUser_ReturnsNotFound()
        {
            this.uri = UsersController.UriBase + Guid.NewGuid();

            using (var response = this.GetAsync().Result)
            {
                // Assert
                response.Should().NotBeNull();
                response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// The get user there are one users returns user
        /// </summary>
        [Test]
        public void GetUser_ThereAreUser_ReturnsUser()
        {
            this.uri = UsersController.UriBase + TestsDb.UserOriginal.UserId;
            using (var response = this.GetAsync().Result)
            {
                // Assert
                response.Should().NotBeNull();
                response.ShouldBeEquivalentTo(TestsDb.UserOriginal);
            }
        }

        /// <summary>
        /// The update user if the user is correct then returns no data.
        /// </summary>
        [Test]
        public void UpdateUser_UserIsCorrect_ReturnsNoData()
        {
            // Arrange
            var userToModify = this.GetUser(TestsDb.UserDefaultTest.UserId);
            userToModify.UserName = "Modified User";

            this.uri = UsersController.UriBase + userToModify.UserId;

            // Action
            using (var response = this.PutAsync(userToModify).Result)
            {
                // Assert Response
                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);

                // Assert DB
                var user = this.GetUser(BaseServerTest.TestsDb.UserDefaultTest.UserId);
                user.UserName.Should().Be(userToModify.UserName);
            }
        }

        /// <summary>
        /// If one user update a user and another user try to update with older data the second update returns error
        /// </summary>
        //[Test]
        //public void UpdateUser_UserIsAlreadyUpdated_ReturnsConcurrencyError()
        //{
        //    // Arrange
        //    var userToModify = this.GetUser(TestsDb.UserDefaultTest.UserId);
        //    userToModify.Name = "Modified User";

        //    this.uri = UsersController.UriBase + userToModify.UserId;

        //    // Action
        //    using (var response = this.PutAsync(userToModify).Result)
        //    {
        //        // Assert Response
        //        response.Should().NotBeNull();
        //        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        //    }

        //    var olderUser = new UserResource().MapFrom(TestsDb.UserDefaultTest);
        //    olderUser.Name = "Concurrent error";

        //    this.uri = UsersController.UriBase + olderUser.UserId;

        //    // Action Try to Update client to older values
        //    using (var response = this.PutAsync(olderUser).Result)
        //    {
        //        // Assert Response
        //        response.Should().NotBeNull();
        //        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        //    }
        //}



        /// <summary>
        /// The addNew user if the user is correct then returns created.
        /// </summary>
        [Test]
        public void InsertUser_UserIsCorrect_ReturnsCreated()
        {
            // Arrange
            var userToAdd = UserTestExtension.GetDefault();

            this.uri = UsersController.UriBase;

            // Action
            using (var response = this.PostAsync(userToAdd).Result)
            {
                // Assert Response
                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.Created, "the user with mail {0} has to be created but fails for {1}", userToAdd.Email, response.Content.ReadAsStringAsync().Result);
                UserTestExtension.ShouldBeEquivalentTo(response, userToAdd);


                // Assert DB
                var userSaved = this.GetUser(response.GetUserResource().UserId);
                UserTestExtension.ShouldBeEquivalentTo(response, userSaved);
            }
        }

        /// <summary>
        /// The addNew user if the user is correct then returns created.
        /// </summary>
        //[Test]
        //public void InsertUser_UserIsCorrect_AuthorizationDataIsCreated()
        //{
        //    // Arrange
        //    var userToAdd = UserTestExtension.GetDefault();

        //    this.uri = UsersController.UriBase;

        //    // Action
        //    using (var response = this.PostAsync(userToAdd).Result)
        //    {
        //        // Assert Response
        //        response.Should().NotBeNull();
        //        response.StatusCode.Should().Be(HttpStatusCode.Created, "the user fails for {0}", response.Content.ReadAsStringAsync().Result);

        //        // Assert DB
        //        var userRedirect = JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result);
        //        var authUser = this.authRepository.FindAsync(userRedirect.MapTo()).Result;
        //        authUser.UserName.ShouldBeEquivalentTo(userRedirect.Name);
        //    }
        //}

        /// <summary>
        /// The addNew user if the user is exists then returns error.
        /// </summary>

        //[Test]
        //public void InsertUser_UserAlreadyExists_ReturnsError()
        //{
        //    // Arrange
        //    var userToAdd = this.GetUser(TestsDb.UserDefaultTest.UserId);
        //    userToAdd.Name = "UserAdded";
        //    userToAdd.Mail = "mail@mail.com";

        //    this.uri = UsersController.UriBase;

        //    // Action
        //    using (var response = this.PostAsync(userToAdd).Result)
        //    {
        //        // Assert Response
        //        response.Should().NotBeNull();
        //        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        //    }
        //}

        /// <summary>
        /// The delete user if the user is correct then returns delete.
        /// </summary>
        [Test]
        public void DeleteUser_UserIsCorrect_ReturnsDeleted()
        {
            // Arrange
            var userToAdd = UserTestExtension.GetDefault();

            this.uri = UsersController.UriBase;
            UserResource userRedirect;

            using (var response = this.PostAsync(userToAdd).Result)
            {
                response.Should().NotBeNull();
                userRedirect = response.GetUserResource();
            }

            this.uri = UsersController.UriBase + userRedirect.UserId;

            // Action
            using (var response = this.DeleteAsync(userRedirect).Result)
            {
                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                var user = response.GetUserResource();
                user.UserId.Should().Be(userRedirect.UserId);

                // AssertBD
                var userDeleted = this.GetUser(user.UserId);
                userDeleted.Should().BeNull();
            }
        }

        /// <summary>
        /// The get user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="UserResource"/>.
        /// </returns>
        private UserResource GetUser(Guid userId)
        {
            this.uri = UsersController.UriBase + userId;
            using (var response = this.GetAsync().Result)
            {
                response.Should().NotBeNull();
                return response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<UserResource>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
        }
    }
}
