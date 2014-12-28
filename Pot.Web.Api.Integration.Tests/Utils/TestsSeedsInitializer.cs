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
            this.AddTestsProjects(context);
        }

        private User userOriginal;
        public User UserOriginal
        {
            get
            {
                return this.userOriginal
                       ?? (this.userOriginal = new User { Email = "original@test.com", UserName = "original", UserId = Guid.NewGuid() });
            }
        }

        private User userDefaultTest;
        public User UserDefaultTest
        {
            get
            {
                return this.userDefaultTest
                       ?? (this.userDefaultTest = new User { Email = "test1@test.com", UserName  = "tes1", UserId = Guid.NewGuid() });
            }
        }

        private Project projectDefaultTest;
        public Project ProjectDefaultTest
        {
            get
            {
                return this.projectDefaultTest
                       ?? (this.projectDefaultTest = new Project { Name = "project test", ProjectId = Guid.NewGuid() });
            }
        }

        private void AddTestsUsers(PotDbContext context)
        {
           var user2 = new User { Email = "test2@test.com", UserName = "tes2", UserId = Guid.NewGuid() };

            context.AppUsers.Add(this.UserDefaultTest);
            context.AppUsers.Add(user2);
            context.AppUsers.Add(this.UserOriginal);
        }

        private void AddTestsProjects(PotDbContext context)
        {
            context.Projects.Add(this.ProjectDefaultTest);
        }
    }
}