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
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Newtonsoft.Json;

    using NUnit.Framework;

    using Pot.Data.BoundedContext.Pot;
    using Pot.Data.Infraestructure;
    using Pot.Data.Model;
    using Pot.Data.SQLServer;
    using Pot.Web.Api.Controllers;
    using Pot.Web.Api.Integration.Tests.Utils;
    using Pot.Web.Api.Model;

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

        ///// <summary>
        ///// The get customer if there are no customers then returns not found.
        ///// </summary>
        //[Test]
        //public void GetCustomer_ThereAreNoCustomer_ReturnsNotFound()
        //{
        //    this.uri = CustomersController.UriBase + Guid.NewGuid();

        //    using (var response = this.GetAsync().Result)
        //    {
        //        // Assert
        //        response.Should().NotBeNull();
        //        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
        //    }
        //}

        ///// <summary>
        ///// The get customer there are one customers returns customer
        ///// </summary>
        //[Test]
        //public void GetCustomer_ThereAreCustomer_ReturnsCustomer()
        //{
        //    this.uri = CustomersController.UriBase + TestsDb.CustomerWithoutModificationsTest.IdCustomer;
        //    using (var response = this.GetAsync(Language.English).Result)
        //    {
        //        // Assert
        //        response.Should().NotBeNull();
        //        var customerResource = CustomerResourceTestExtension.GetCustomerResource(response);
        //        customerResource.ShouldBeEquivalentToCustomer(TestsDb.CustomerWithoutModificationsTest, Language.English);
        //    }
        //}

        ///// <summary>
        ///// The get customer set language spanish there are customer returns customer with country and payment in spanish.
        ///// </summary>
        //[Test]
        //public void GetCustomerSetLanguageSpanish_ThereAreCustomer_ReturnsCustomerWithCountryAndPaymentInSpanish()
        //{
        //    // Arrange
        //    this.uri = CustomersController.UriBase + TestsDb.CustomerWithoutModificationsTest.IdCustomer;
        //    using (var response = this.GetAsync(Language.Spanish).WithCurrentCulture().GetResult())
        //    {
        //        // Assert
        //        response.Should().NotBeNull();
        //        var customerResource = CustomerResourceTestExtension.GetCustomerResource(response);
        //        customerResource.ShouldBeEquivalentToCustomer(
        //            BaseServerTest.TestsDb.CustomerWithoutModificationsTest,
        //            Language.Spanish);
        //    }
        //}

        ///// <summary>
        ///// The update customer if the customer is correct then returns no data.
        ///// </summary>
        //[Test]
        //public void UpdateCustomer_CustomerIsCorrect_ReturnsNoData()
        //{
        //    // Arrange
        //    var customerToModify = this.GetCustomer(BaseServerTest.TestsDb.DefaultCustomerTest.IdCustomer);
        //    customerToModify.Name = "Modified Customer";

        //    this.uri = CustomersController.UriBase + customerToModify.idCustomer;

        //    // Action
        //    using (var response = this.PutAsync(customerToModify).Result)
        //    {
        //        // Assert Response
        //        response.Should().NotBeNull();
        //        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        //        // Assert DB
        //        var customer = this.GetCustomer(BaseServerTest.TestsDb.DefaultCustomerTest.IdCustomer);
        //        customer.Name.Should().Be(customerToModify.Name);
        //    }
        //}

        ///// <summary>
        ///// The update customer if the customer and the user culture is english is correct then returns error in english.
        ///// </summary>
        ///// <param name="language">
        ///// The language.
        ///// </param>
        ///// <param name="test">
        ///// The test.
        ///// </param>
        //[TestCase(Language.English, "field")]
        //[TestCase(Language.Spanish, "campo")]
        //public void UpdateCustomer_CustomerIsIncorrect_ReturnsErrorTextInUserLanguage(string language, string test)
        //{
        //    // Arrange
        //    var customerToModify = this.GetCustomer(BaseServerTest.TestsDb.DefaultCustomerTest.IdCustomer);
        //    customerToModify.Name = string.Empty;
        //    customerToModify.surname1 = string.Empty;

        //    this.uri = CustomersController.UriBase + customerToModify.idCustomer;

        //    // Action
        //    using (var response = this.PutAsync(customerToModify, language).Result)
        //    {
        //        // Assert Response
        //        response.Should().NotBeNull();
        //        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        //        var errors = ErrorsTestExtension.GetErrors(response);
        //        errors.Count(c => c.Contains(test)).Should().BeGreaterOrEqualTo(2);
        //    }
        //}

        ///// <summary>
        ///// If one user update a customer and another user try to update with older data the second update returns error
        ///// </summary>
        //[Test]
        //public void UpdateCustomer_CustomerIsAlreadyUpdated_ReturnsConcurrencyError()
        //{
        //    // Arrange
        //    var customerToModify = this.GetCustomer(TestsDb.DefaultCustomerTest.IdCustomer);
        //    customerToModify.Name = "Modified Customer";

        //    this.uri = CustomersController.UriBase + customerToModify.idCustomer;

        //    // Action
        //    using (var response = this.PutAsync(customerToModify).Result)
        //    {
        //        // Assert Response
        //        response.Should().NotBeNull();
        //        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        //    }

        //    var olderCustomer = new CustomerResource().MapFrom(TestsDb.DefaultCustomerTest);
        //    olderCustomer.Name = "Concurrent error";

        //    this.uri = CustomersController.UriBase + olderCustomer.idCustomer;

        //    // Action Try to Update client to older values
        //    using (var response = this.PutAsync(olderCustomer).Result)
        //    {
        //        // Assert Response
        //        response.Should().NotBeNull();
        //        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        //    }
        //}

        ///// <summary>
        ///// Check that the fields update user and date updated has been updated
        ///// </summary>
        //[Test]
        //public void UpdateCustomer_CustomerIsCorrect_UpdatedUserAndUpdatedDataHasBeenUpdated()
        //{
        //    // Arrange
        //    var customerToModify = this.GetCustomer(BaseServerTest.TestsDb.DefaultCustomerTest.IdCustomer);
        //    customerToModify.Name = "Modified Customer";
        //    var updatedDate = DateTime.Now;

        //    this.uri = CustomersController.UriBase + customerToModify.idCustomer;

        //    // Action
        //    using (var response = this.PutAsync(customerToModify).Result)
        //    {
        //        // Assert Response
        //        response.Should().NotBeNull();
        //        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        //    }

        //    // Assert DB
        //    var customer = this.GetCustomer(BaseServerTest.TestsDb.DefaultCustomerTest.IdCustomer);
        //    customer.Name.Should().Be(customerToModify.Name);
        //    customer.updatedDate.Should().BeOnOrAfter(updatedDate);
        //    customer.updatedUser.Should().NotBeNullOrWhiteSpace();
        //}

        ///// <summary>
        ///// The addNew customer if the customer is incorrect then returns error in user language.
        ///// </summary>
        ///// <param name="language">
        ///// The language.
        ///// </param>
        ///// <param name="text">
        ///// The text.
        ///// </param>
        //[TestCase(Language.English, "field")]
        //[TestCase(Language.Spanish, "campo")]
        //public void InsertCustomer_CustomerIsIncorrect_ReturnsErrorInUserLanguage(string language, string text)
        //{
        //    // Arrange
        //    var customerToAdd = new CustomerResource
        //    {
        //        Name = "Customer Added",
        //        surname1 = "surname",
        //    };

        //    this.uri = CustomersController.UriBase;

        //    // Action
        //    using (var response = this.PostAsync(customerToAdd, language).Result)
        //    {
        //        // Assert Response
        //        response.Should().NotBeNull();
        //        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        //        var errors = ErrorsTestExtension.GetErrors(response);

        //        errors.Count(c => c.Contains(text)).Should().BeGreaterOrEqualTo(1);
        //    }
        //}

        ///// <summary>
        ///// The addNew customer if the customer is correct then returns created.
        ///// </summary>
        //[Test]
        //public void InsertCustomer_CustomerIsCorrect_ReturnsCreated()
        //{
        //    // Arrange
        //    var customerToAdd = CustomerResourceTestExtension.GetDefault();

        //    this.uri = CustomersController.UriBase;

        //    // Action
        //    using (var response = this.PostAsync(customerToAdd).Result)
        //    {
        //        // Assert Response
        //        response.Should().NotBeNull();
        //        response.StatusCode.Should().Be(HttpStatusCode.Created, "the customer with mail {0} has to be created but fails for {1}", customerToAdd.eMail, response.Content.ReadAsStringAsync().Result);
        //        var customerRedirect = CustomerResourceTestExtension.GetCustomerResource(response);
        //        customerRedirect.eMail.Should().Be(customerToAdd.eMail);

        //        // Assert DB
        //        var customerSaved = this.GetCustomer(customerRedirect.idCustomer);
        //        customerRedirect.ShouldBeEquivalentToCustomer(customerSaved);
        //    }
        //}

        ///// <summary>
        ///// The addNew customer if the customer is correct then returns created.
        ///// </summary>
        //[Test]
        //public void InsertCustomer_CustomerIsCorrect_AuthorizationDataIsCreated()
        //{
        //    // Arrange
        //    var customerToAdd = CustomerResourceTestExtension.GetDefault();

        //    this.uri = CustomersController.UriBase;

        //    // Action
        //    using (var response = this.PostAsync(customerToAdd).Result)
        //    {
        //        // Assert Response
        //        response.Should().NotBeNull();
        //        response.StatusCode.Should().Be(HttpStatusCode.Created, "the customer fails for {0}", response.Content.ReadAsStringAsync().Result);

        //        // Assert DB
        //        var customerRedirect = JsonConvert.DeserializeObject<CustomerResource>(response.Content.ReadAsStringAsync().Result);
        //        var authUser = this.authRepository.FindAsync(customerRedirect.MapTo()).Result;
        //        authUser.UserName.ShouldBeEquivalentTo(customerRedirect.Name);
        //    }
        //}

        ///// <summary>
        ///// The addNew customer if the customer is exists then returns error.
        ///// </summary>
        //[Test]
        //public void InsertCustomer_CustomerAlreadyExists_ReturnsError()
        //{
        //    // Arrange
        //    var customerToAdd = this.GetCustomer(BaseServerTest.TestsDb.DefaultCustomerTest.IdCustomer);
        //    customerToAdd.Name = "CustomerAdded";
        //    customerToAdd.eMail = "mail@mail.com";

        //    this.uri = CustomersController.UriBase;

        //    // Action
        //    using (var response = this.PostAsync(customerToAdd).Result)
        //    {
        //        // Assert Response
        //        response.Should().NotBeNull();
        //        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        //    }
        //}

        ///// <summary>
        ///// The delete customer if the customer is correct then returns delete.
        ///// </summary>
        //[Test]
        //public void DeleteCustomer_CustomerIsCorrect_ReturnsDeleted()
        //{
        //    // Arrange
        //    var customerToAdd = CustomerResourceTestExtension.GetDefault();

        //    this.uri = CustomersController.UriBase;
        //    CustomerResource customerRedirect;

        //    using (var response = this.PostAsync(customerToAdd).Result)
        //    {
        //        response.Should().NotBeNull();
        //        customerRedirect = CustomerResourceTestExtension.GetCustomerResource(response);
        //    }

        //    this.uri = CustomersController.UriBase + customerRedirect.idCustomer;

        //    // Action
        //    using (var response = this.DeleteAsync(customerRedirect).Result)
        //    {
        //        response.Should().NotBeNull();
        //        response.StatusCode.Should().Be(HttpStatusCode.OK);
        //        var customer = CustomerResourceTestExtension.GetCustomerResource(response);
        //        customer.idCustomer.Should().Be(customerRedirect.idCustomer);

        //        // AssertBD
        //        var customerDeleted = this.GetCustomer(customer.idCustomer);
        //        customerDeleted.Should().BeNull();
        //    }
        //}

        ///// <summary>
        ///// The get customer.
        ///// </summary>
        ///// <param name="customerId">
        ///// The customer id.
        ///// </param>
        ///// <returns>
        ///// The <see cref="CustomerResource"/>.
        ///// </returns>
        //private CustomerResource GetCustomer(Guid customerId)
        //{
        //    this.uri = CustomersController.UriBase + customerId;
        //    using (var response = this.GetAsync().Result)
        //    {
        //        response.Should().NotBeNull();
        //        return response.StatusCode == HttpStatusCode.OK
        //            ? JsonConvert.DeserializeObject<CustomerResource>(response.Content.ReadAsStringAsync().Result)
        //            : null;
        //    }
        //}

        // TODO Al crear un cliente si el usuario esta autentificado se graba como usuario de creacion el usuario de la sesion
    }
}
