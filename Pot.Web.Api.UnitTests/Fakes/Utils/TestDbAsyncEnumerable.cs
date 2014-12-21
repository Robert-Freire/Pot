namespace Pot.Web.Api.Unit.Tests
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// The test database async enumerable.
    /// </summary>
    /// <typeparam name="T">
    /// Entity class
    /// </typeparam>
    internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestDbAsyncEnumerable{T}"/> class.
        /// </summary>
        /// <param name="enumerable">
        /// The enumerable.
        /// </param>
        public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestDbAsyncEnumerable{T}"/> class.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        public TestDbAsyncEnumerable(Expression expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Gets the provider.
        /// </summary>
        IQueryProvider IQueryable.Provider
        {
            get { return new TestDbAsyncQueryProvider<T>(this); }
        }

        /// <summary>
        /// The get async enumerator.
        /// </summary>
        /// <returns>
        /// The <see cref="IDbAsyncEnumerator"/>.
        /// </returns>
        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        /// <summary>
        /// The get async enumerator.
        /// </summary>
        /// <returns>
        /// The <see cref="IDbAsyncEnumerator"/>.
        /// </returns>
        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return this.GetAsyncEnumerator();
        }
    }
}