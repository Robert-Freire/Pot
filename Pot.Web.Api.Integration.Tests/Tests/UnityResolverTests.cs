namespace Pot.Web.Api.Integration.Tests
{
    using System.Web.Http;
    using System.Web.Http.Dependencies;

    using FluentAssertions;

    using NUnit.Framework;

    using Pot.Data;
    using Pot.Data.SQLServer;
    using Pot.Web.Api.Controllers;

    /// <summary>
    /// The unity resolver tests.
    /// </summary>
    [TestFixture]
    public class UnityResolverTests
    {
        /// <summary>
        /// The resolver.
        /// </summary>
        private readonly IDependencyResolver resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityResolverTests"/> class.
        /// </summary>
        public UnityResolverTests()
        {
            var config = new HttpConfiguration();
            config.RegisterUnityComponents();
            this.resolver = config.DependencyResolver;
        }

        [Test]
        public void UserFactory_IsResolved_GetUserFactory()
        {
            var authFactory = this.resolver.GetService(typeof(IUserFactory));
            authFactory.Should().BeOfType<UserFactory>();
        }

        [Test]
        public void UserController_IsResolved_GetAUserController()
        {
            var customerController = this.resolver.GetService(typeof(UsersController));
            customerController.Should().BeOfType<UsersController>();
        }

        [Test]
        public void ProjectController_IsResolved_GetAProjectController()
        {
            var projectController = this.resolver.GetService(typeof(ProjectsController));
            projectController.Should().BeOfType<ProjectsController>();
        }
    }
}
