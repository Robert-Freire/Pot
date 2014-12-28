namespace Pot.Data.SQLServer.Utis
{
    using System;
    using System.Data.Entity;

    using Pot.Data.Infraestructure;

    /// <summary>
    /// The customer factory.
    /// </summary>
    public class ContextFactory : IContextFactoryAsync
    {

        private readonly PotDbContext potDbContext;

        protected PotDbContext PotDbContext
        {
            get
            {
                return this.potDbContext;
            }
        }

        /// <summary>
        /// The unit of work async.
        /// </summary>
        private IUnitOfWorkAsync unitOfWorkAsync;
        public ContextFactory(PotDbContext potDbContext)
        {
            this.potDbContext = potDbContext;
        }

        /// <summary>
        /// Gets or sets the unit of work async.
        /// </summary>
        public virtual IUnitOfWorkAsync UnitOfWorkAsync
        {
            get
            {
                return this.unitOfWorkAsync ?? (this.unitOfWorkAsync = new UnitOfWork(this.potDbContext));
            }

            protected set
            {
                this.unitOfWorkAsync = value;
            }
        }

        /// <summary>
        /// Gets the db context.
        /// </summary>
        protected DbContext DbContext
        {
            get
            {
                return this.potDbContext;
            }
        }

        /// <summary>
        /// The get repository async.
        /// </summary>
        /// <typeparam name="T">
        /// Base class for the repository
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public virtual IRepositoryAsync<T> GetRepositoryAsync<T>() where T : class
        {
            return new Repository<T>(this.potDbContext);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.unitOfWorkAsync != null)
                {
                    this.unitOfWorkAsync.Dispose();
                }

                this.potDbContext.Dispose();
            }
        }
    }
}
