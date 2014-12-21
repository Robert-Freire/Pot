// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbSetMock.cs" company="Nova Language Services">
//   Copyright © Nova Language Services 2014
// </copyright>
// <summary>
//   Defines the DbSetMock type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Pot.Web.Api.Unit.Tests
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;

    using Moq;

    /// <summary>
    /// The database set mock.
    /// </summary>
    /// <typeparam name="TEntity">
    /// Entity of the set to mock
    /// </typeparam>
    internal class DbSetMock<TEntity> : Mock<DbSet<TEntity>>
        where TEntity : class
    {
        /// <summary>
        /// The setup.
        /// </summary>
        /// <param name="queryable">
        /// The list.
        /// </param>
        public void Setup(IQueryable<TEntity> queryable)
        {
            if (queryable == null)
            {
                return;
            }

            this.As<IDbAsyncEnumerable<TEntity>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<TEntity>(queryable.GetEnumerator()));

            this.As<IQueryable<TEntity>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<TEntity>(queryable.Provider));

            this.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(queryable.Expression);
            this.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            this.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            this.Setup(m => m.Include(It.IsAny<string>())).Returns(this.Object);
        }
    }
}
