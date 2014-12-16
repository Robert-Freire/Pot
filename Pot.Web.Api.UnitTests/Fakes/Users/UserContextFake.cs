//// --------------------------------------------------------------------------------------------------------------------
//// <copyright file="UserContextFake.cs" company="Nova Language Services">
////   Copyright © Nova Language Services 2014
//// </copyright>
//// <summary>
////   The user context mock.
//// </summary>
//// --------------------------------------------------------------------------------------------------------------------

//namespace Pot.Web.Api.Unit.Tests
//{
//    using System.Linq;

//    using Moq;

//    using Pot.Data.Model;
//    using Pot.Data.SQLServer;

//    /// <summary>
//    /// The user context mock.
//    /// </summary>
//    internal class UserContextFake : Mock<PotDbContext>
//    {
//        /// <summary>
//        /// The setup sets.
//        /// </summary>
//        /// <param name="users">
//        /// The users.
//        /// </param>
//        public void SetupSets(IQueryable<User> users)
//        {
//            var userSetMock = new DbSetMock<User>();
//            userSetMock.Setup(users);

//            userSetMock.Setup(m => m.Include(It.IsAny<string>())).Returns(userSetMock.Object);

//            this.Setup(c => c.Set<User>()).Returns(userSetMock.Object);
//        }
//    }
//}
