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

        private User userOriginal;
        public User UserOriginal
        {
            get
            {
                return this.userOriginal
                       ?? (this.userOriginal = new User { Mail = "original@test.com", Name = "original", UserId = Guid.NewGuid() });
            }
        }

        private User userDefaultTest;
        public User UserDefaultTest
        {
            get
            {
                return this.userDefaultTest
                       ?? (this.userDefaultTest = new User { Mail = "test1@test.com", Name = "tes1", UserId = Guid.NewGuid() });
            }
        }
        /// <summary>
        /// The customers tests seeds initializer.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        private void AddTestsUsers(PotDbContext context)
        {
           var user2 = new User { Mail = "test2@test.com", Name = "tes2", UserId = Guid.NewGuid() };

            context.Users.Add(this.UserDefaultTest);
            context.Users.Add(user2);
            context.Users.Add(this.UserOriginal);
        }
    }
}