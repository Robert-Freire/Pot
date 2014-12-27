// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerRepositoryFake.cs" company="Nova Language Services">
//   Copyright © Nova Language Services 2014
// </copyright>
// <summary>
//   The customer repository mock.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Pot.Web.Api.Unit.Tests
{
    using System.Threading;
    using System.Threading.Tasks;

    using Moq;

    using Pot.Data.Infraestructure;
    using Pot.Data.Model;
    using Pot.Data.SQLServer;
    using Pot.Data.SQLServer.Utis;

    internal class UserRepositoryFake : Repository<User>
    {
        /// <summary>
        /// The customer repository mock.
        /// </summary>
        private readonly Mock<IRepositoryAsync<User>> userRepositoryMock;


        public UserRepositoryFake(PotDbContext dbContext, Mock<IRepositoryAsync<User>> userRepositoryMock = null)
            : base(dbContext)
        {
            this.userRepositoryMock = userRepositoryMock;
        }

        /// <summary>
        /// The find async.
        /// </summary>
        /// <param name="keyValues">
        /// The key values.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override Task<User> FindAsync(params object[] keyValues)
        {
            return this.userRepositoryMock == null ? base.FindAsync(keyValues) : this.userRepositoryMock.Object.FindAsync(keyValues);
        }

        /// <summary>
        /// The find async.
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation Token.
        /// </param>
        /// <param name="keyValues">
        /// The key values.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override Task<User> FindAsync(CancellationToken cancellationToken, object[] keyValues)
        {
            return this.userRepositoryMock == null ? base.FindAsync(cancellationToken, keyValues) : this.userRepositoryMock.Object.FindAsync(cancellationToken, keyValues);
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public override void Delete(User entity)
        {
            if (this.userRepositoryMock == null)
            {
                base.Delete(entity);
            }
            else
            {
                this.userRepositoryMock.Object.Delete(entity);
            }
        }

        /// <summary>
        /// The set modified.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void SetModified(User entity)
        {
        }
    }
}
