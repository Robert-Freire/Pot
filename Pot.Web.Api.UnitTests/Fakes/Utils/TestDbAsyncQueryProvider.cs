namespace Pot.Web.Api.Unit.Tests
{
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// The test database async query provider.
    /// </summary>
    /// <typeparam name="TEntity">
    /// Entity class
    /// </typeparam>
    internal class TestDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider
    {
        /// <summary>
        /// The _inner.
        /// </summary>
        private readonly IQueryProvider inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestDbAsyncQueryProvider{TEntity}"/> class.
        /// </summary>
        /// <param name="inner">
        /// The inner.
        /// </param>
        internal TestDbAsyncQueryProvider(IQueryProvider inner)
        {
            this.inner = inner;
        }

        /// <summary>
        /// The create query.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable CreateQuery(Expression expression)
        {
            return new TestDbAsyncEnumerable<TEntity>(expression);
        }

        /// <summary>
        /// The create query.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <typeparam name="TElement">
        /// Element class
        /// </typeparam>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestDbAsyncEnumerable<TElement>(expression);
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object Execute(Expression expression)
        {
            return this.inner.Execute(expression);
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <typeparam name="TResult">
        /// Result class
        /// </typeparam>
        /// <returns>
        /// The <see cref="TResult"/>.
        /// </returns>
        public TResult Execute<TResult>(Expression expression)
        {
            return this.inner.Execute<TResult>(expression);
        }

        /// <summary>
        /// The execute async.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(this.Execute(expression));
        }

        /// <summary>
        /// The execute async.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <typeparam name="TResult">
        /// Result class
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(this.Execute<TResult>(expression));
        }
    }
}