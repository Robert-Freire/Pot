// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestDbAsyncEnumerator.cs" company="Nova Language Services">
//   Copyright © Nova Language Services 2014
// </copyright>
// <summary>
//   Defines the TestDbAsyncQueryProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Pot.Web.Api.Unit.Tests
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// The test database async enumerator.
    /// </summary>
    /// <typeparam name="T">
    /// Entity class
    /// </typeparam>
    internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        /// <summary>
        /// The inner.
        /// </summary>
        private readonly IEnumerator<T> inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestDbAsyncEnumerator{T}"/> class.
        /// </summary>
        /// <param name="inner">
        /// The inner.
        /// </param>
        public TestDbAsyncEnumerator(IEnumerator<T> inner)
        {
            this.inner = inner;
        }

        /// <summary>
        /// Gets the current.
        /// </summary>
        public T Current
        {
            get { return this.inner.Current; }
        }

        /// <summary>
        /// Gets the current.
        /// </summary>
        object IDbAsyncEnumerator.Current
        {
            get { return this.Current; }
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.inner.Dispose();
        }

        /// <summary>
        /// The move next async.
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(this.inner.MoveNext());
        }
    }
}