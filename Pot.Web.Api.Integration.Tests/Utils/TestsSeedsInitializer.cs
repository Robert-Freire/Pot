// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestsSeedsInitializer.cs" company="Nova Language Services">
//   Copyright © Nova Language Services 2014
// </copyright>
// <summary>
//   The customers test seeds initializer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Pot.Web.Api.Integration.Tests.Utils
{
    using System;
    using System.Data.Entity;

    using Pot.Data.Model;
    using Pot.Data.SQLServer;

    /// <summary>
    /// The customers test seeds initializer.
    /// </summary>
    public class TestsSeedsInitializer : DropCreateDatabaseAlways<PotDbContext>
    {
        /// <summary>
        /// The seed.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        protected override void Seed(PotDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.AddTestsUsers(context);
        }

        /// <summary>
        /// The customers tests seeds initializer.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        private void AddTestsUsers(PotDbContext context)
        {
            var user1 = new User { Mail = "test1@test.com", Name = "tes1", UserId = Guid.NewGuid() };
            var user2 = new User { Mail = "test2@test.com", Name = "tes2", UserId = Guid.NewGuid() };

            context.Users.Add(user1);
            context.Users.Add(user2);
        }
    }
}