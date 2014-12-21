// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersMappingsTests.cs" company="Nova Language Services">
//   Copyright © Nova Language Services 2014
// </copyright>
// <summary>
//   The users mappings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Pot.Web.Api.Unit.Tests
{
    using System;

    using FluentAssertions;

    using NUnit.Framework;

    using Pot.Data.Model;
    using Pot.Web.Api.Model;

    /// <summary>
    /// The users mappings.
    /// </summary>
    [TestFixture]
    public class UsersMappingsTests
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersMappingsTests"/> class.
        /// </summary>
        public UsersMappingsTests()
        {
            AutomapperConfig.Initialize();
        }

        [Test]
        public void Users_MapToResource_ResourceIsMapped()
        {
            // Arrange
            var user = new User { Name = "User", UserId = Guid.NewGuid() };

            // Action
            var userResource = new UserResource().MapFrom(user);

            // Assert
            userResource.ShouldBeEquivalentToUser(user);
        }

        /// <summary>
        /// Check the map from user resource to user
        /// </summary>
        [Test]
        public void UsersResource_MapToUser_UserIsmapped()
        {
            // Arrange
            var userResource = new UserResource { Name = "Cliente1" };

            // Act
            var user = userResource.MapTo();

            // Assert
            user.ShouldBeEquivalentToUser(userResource);
        }

        /// <summary>
        /// Check the map from user resource to existing user
        /// </summary>
        [Test]
        public void UsersResource_MapToExistingUser_UserIsmapped()
        {
            // Arrange
            var user = new User { UserId = Guid.NewGuid(), Name = "Original name" };
            var userResource = new UserResource { UserId = Guid.NewGuid(), Name = "Modified name" };

            // Action
            var modifiedUser = userResource.MapTo(user);

            // Assert
            modifiedUser.ShouldBeEquivalentToUser(userResource);
            modifiedUser.GetHashCode().Should().Be(user.GetHashCode());
        }
    }
}
