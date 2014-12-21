namespace Pot.Data.SQLServer.Utis
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Transactions;

    using Pot.Data.Infraestructure;

    /// <summary>
    /// The unit of work.
    /// </summary>
    public class UnitOfWork : IUnitOfWorkAsync
    {
        private readonly DbContext dbContext;

        /// <summary>
        /// The transaction.
        /// </summary>
        private TransactionScope transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The database context.
        /// </param>
        public UnitOfWork(DbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }

            this.dbContext = dbContext;
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
        /// The save changes.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public virtual SaveStatus SaveChanges()
        {
            var saveStatus = new EfSaveStatus();

            try
            {
                saveStatus.UpdatedEntitiesNumber = this.dbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                return saveStatus.SetErrors(ex.EntityValidationErrors);
            }

            return saveStatus;
        }

        /// <summary>
        /// The begin transaction.
        /// </summary>
        public void BeginTransaction()
        {
            this.transaction = new TransactionScope();
        }

        /// <summary>
        /// The commit.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public virtual bool Commit()
        {
            this.dbContext.SaveChanges();
            this.transaction.Complete();
            return true;
        }

        /// <summary>
        /// The rollback.
        /// </summary>
        public void Rollback()
        {
            this.transaction.Dispose();
        }

        /// <summary>
        /// The save changes async.
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public virtual async Task<SaveStatus> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var saveStatus = new EfSaveStatus();

            try
            {
                saveStatus.UpdatedEntitiesNumber = await this.dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbEntityValidationException ex)
            {
                saveStatus.SetErrors(ex.EntityValidationErrors);
            }

            return saveStatus;
        }

        /// <summary>
        /// The save changes async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public virtual async Task<SaveStatus> SaveChangesAsync()
        {
            return await this.SaveChangesAsync(CancellationToken.None);
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
                if (this.transaction != null)
                {
                    this.transaction.Dispose();
                    this.transaction = null;
                }
            }
        }
    }
}
