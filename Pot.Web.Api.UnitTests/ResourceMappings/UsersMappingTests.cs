//// --------------------------------------------------------------------------------------------------------------------
//// <copyright file="UsersMappingsTests.cs" company="Nova Language Services">
////   Copyright © Nova Language Services 2014
//// </copyright>
//// <summary>
////   The users mappings.
//// </summary>
//// --------------------------------------------------------------------------------------------------------------------

//namespace Pot.Web.Api.Unit.Tests
//{
//    using System;
//    using System.Globalization;
//    using System.Threading;

//    using FluentAssertions;

//    using NUnit.Framework;

//    using Pot.Data.Model;
//    using Pot.Web.Api.Model;

//    /// <summary>
//    /// The users mappings.
//    /// </summary>
//    [TestFixture]
//    public class UsersMappingsTests
//    {
//        /// <summary>
//        /// Initializes a new instance of the <see cref="UsersMappingsTests"/> class.
//        /// </summary>
//        public UsersMappingsTests()
//        {
//            AutomapperConfig.Initialize();
//        }

//        /// <summary>
//        /// When the users has currency and we map to resource then resource has currency.
//        /// </summary>
//        [Test]
//        public void UsersHasCurrency_MapToResource_ResourceHasCurrency()
//        {
//            // Arrange
//            var user = new User { Name = "User", UserId = Guid.NewGuid()};

//            // Action
//            var userResource = new UserResource().MapFrom(user);

//            // Assert
//            userResource.ShouldBeEquivalentTo(user);
//        }
       
//        /// <summary>
//        /// Check the map from user resource to user
//        /// </summary>
//        [Test]
//        public void UsersResource_MapToUser_UserIsmapped()
//        {
//            // Arrange
//            var userResource = new UserResource { name = "Cliente1" };

//            // Action
//            var user = userResource.MapTo();

//            // Assert
//            user.ShouldBeEquivalentTo(userResource);
//        }

//        /// <summary>
//        /// Check the map from user resource to existing user
//        /// </summary>
//        [Test]
//        public void UsersResource_MapToExistingUser_UserIsmapped()
//        {
//            // Arrange
//            var user = new User { IdUser = Guid.NewGuid(), Name = "Original name" };
//            var userResource = new UserResource { idUser = Guid.NewGuid(), name = "Modified name" };

//            // Action
//            var modifiedUser = userResource.MapTo(user);

//            // Assert
//            modifiedUser.ShouldBeEquivalentTo(userResource);
//            modifiedUser.GetHashCode().Should().Be(user.GetHashCode());
//        }
//    }
//}
