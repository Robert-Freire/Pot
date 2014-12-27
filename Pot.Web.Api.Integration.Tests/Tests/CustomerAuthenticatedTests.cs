// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerAuthenticatedTests.cs" company="Nova Language Services">
//   Copyright © Nova Language Services 2014
// </copyright>
// <summary>
//   The customer OWIN authenticated tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NWL.Web.API.OWIN.Integration.Tests
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
    using Pot.Web.Api;
    using Pot.Web.Api.Controllers;
    using Pot.Web.Api.Integration.Tests.Utils;
    using Pot.Web.Api.Model;
    using Pot.Web.Api.Unit.Tests;

    /// <summary>
    /// The customer OWIN authenticated tests.
    /// </summary>
    [TestFixture]
    public class CustomerAuthenticatedTests : BaseAuthenticatedTests
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
        /// Initializes a new instance of the <see cref="CustomerAuthenticatedTests"/> class.
        /// </summary>
        public CustomerAuthenticatedTests()
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
        /// The get all customers if there are two customers then returns two customers.
        /// </summary>
        [Test]
        public void GetAllCustomers_ThereAreAlmosTwoCustomers_ReturnsTwoCustomersOrMore()
        {
            this.uri = UsersController.UriBase;

            using (var response = this.GetAsync().Result)
            {
                // Assert
                response.Should().NotBeNull();
                var customers = JsonConvert.DeserializeObject<List<UserResource>>(response.Content.ReadAsStringAsync().Result);
                customers.Count.Should().BeGreaterOrEqualTo(2);
            }
        }

        /// <summary>
        /// The get customer if there are no customers then returns not found.
        /// </summary>
        [Test]
        public void GetCustomer_ThereAreNoCustomer_ReturnsNotFound()
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
        /// The get customer there are one customers returns customer
        /// </summary>
        [Test]
        public void GetCustomer_ThereAreCustomer_ReturnsCustomer()
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
        /// The update customer if the customer is correct then returns no data.
        /// </summary>
        [Test]
        public void UpdateCustomer_CustomerIsCorrect_ReturnsNoData()
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
                var customer = this.GetUser(BaseServerTest.TestsDb.UserDefaultTest.UserId);
                customer.UserName.Should().Be(userToModify.UserName);
            }
        }

        /// <summary>
        /// If one user update a customer and another user try to update with older data the second update returns error
        /// </summary>
        //[Test]
        //public void UpdateCustomer_CustomerIsAlreadyUpdated_ReturnsConcurrencyError()
        //{
        //    // Arrange
        //    var customerToModify = this.GetUser(TestsDb.UserDefaultTest.UserId);
        //    customerToModify.Name = "Modified Customer";

        //    this.uri = UsersController.UriBase + customerToModify.UserId;

        //    // Action
        //    using (var response = this.PutAsync(customerToModify).Result)
        //    {
        //        // Assert Response
        //        response.Should().NotBeNull();
        //        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        //    }

        //    var olderCustomer = new UserResource().MapFrom(TestsDb.UserDefaultTest);
        //    olderCustomer.Name = "Concurrent error";

        //    this.uri = UsersController.UriBase + olderCustomer.UserId;

        //    // Action Try to Update client to older values
        //    using (var response = this.PutAsync(olderCustomer).Result)
        //    {
        //        // Assert Response
        //        response.Should().NotBeNull();
        //        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        //    }
        //}



        /// <summary>
        /// The addNew customer if the customer is correct then returns created.
        /// </summary>
        [Test]
        public void InsertCustomer_CustomerIsCorrect_ReturnsCreated()
        {
            // Arrange
            var userToAdd = UserTestExtension.GetDefault();

            this.uri = UsersController.UriBase;

            // Action
            using (var response = this.PostAsync(userToAdd).Result)
            {
                // Assert Response
                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.Created, "the customer with mail {0} has to be created but fails for {1}", userToAdd.Email, response.Content.ReadAsStringAsync().Result);
                response.ShouldBeEquivalentTo(userToAdd);


                // Assert DB
                var customerSaved = this.GetUser(response.GetUserResource().UserId);
                response.ShouldBeEquivalentTo(customerSaved);
            }
        }

        /// <summary>
        /// The addNew customer if the customer is correct then returns created.
        /// </summary>
        //[Test]
        //public void InsertCustomer_CustomerIsCorrect_AuthorizationDataIsCreated()
        //{
        //    // Arrange
        //    var customerToAdd = UserTestExtension.GetDefault();

        //    this.uri = UsersController.UriBase;

        //    // Action
        //    using (var response = this.PostAsync(customerToAdd).Result)
        //    {
        //        // Assert Response
        //        response.Should().NotBeNull();
        //        response.StatusCode.Should().Be(HttpStatusCode.Created, "the customer fails for {0}", response.Content.ReadAsStringAsync().Result);

        //        // Assert DB
        //        var customerRedirect = JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result);
        //        var authUser = this.authRepository.FindAsync(customerRedirect.MapTo()).Result;
        //        authUser.UserName.ShouldBeEquivalentTo(customerRedirect.Name);
        //    }
        //}

        /// <summary>
        /// The addNew customer if the customer is exists then returns error.
        /// </summary>

        //[Test]
        //public void InsertCustomer_CustomerAlreadyExists_ReturnsError()
        //{
        //    // Arrange
        //    var customerToAdd = this.GetUser(TestsDb.UserDefaultTest.UserId);
        //    customerToAdd.Name = "CustomerAdded";
        //    customerToAdd.Mail = "mail@mail.com";

        //    this.uri = UsersController.UriBase;

        //    // Action
        //    using (var response = this.PostAsync(customerToAdd).Result)
        //    {
        //        // Assert Response
        //        response.Should().NotBeNull();
        //        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        //    }
        //}

        /// <summary>
        /// The delete customer if the customer is correct then returns delete.
        /// </summary>
        [Test]
        public void DeleteCustomer_CustomerIsCorrect_ReturnsDeleted()
        {
            // Arrange
            var customerToAdd = UserTestExtension.GetDefault();

            this.uri = UsersController.UriBase;
            UserResource customerRedirect;

            using (var response = this.PostAsync(customerToAdd).Result)
            {
                response.Should().NotBeNull();
                customerRedirect = response.GetUserResource();
            }

            this.uri = UsersController.UriBase + customerRedirect.UserId;

            // Action
            using (var response = this.DeleteAsync(customerRedirect).Result)
            {
                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                var customer = response.GetUserResource();
                customer.UserId.Should().Be(customerRedirect.UserId);

                // AssertBD
                var customerDeleted = this.GetUser(customer.UserId);
                customerDeleted.Should().BeNull();
            }
        }

        /// <summary>
        /// The get customer.
        /// </summary>
        /// <param name="userId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The <see cref="CustomerResource"/>.
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

        // TODO Al crear un cliente si el usuario esta autentificado se graba como usuario de creacion el usuario de la sesion
    }
}
