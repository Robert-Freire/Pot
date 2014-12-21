// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Repository.cs" company="Nova Language Services">
//   Copyright © Nova Language Services 2014
// </copyright>
// <summary>
//   Base class for all SQL based service classes
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Pot.Data.SQLServer.Utis
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    using Pot.Data.Infraestructure;

    /// <summary>
    /// Base class for all SQL based service classes
    /// </summary>
    /// <typeparam name="TEntity">The domain object type</typeparam>
    public class Repository<TEntity> : IRepositoryAsync<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// The database context.
        /// </summary>
        private readonly DbContext dbContext;

        /// <summary>
        /// The database set.
        /// </summary>
        private readonly DbSet<TEntity> dbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The database context.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If database context is null
        /// </exception>
        public Repository(DbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }

            this.dbContext = dbContext;

            this.dbSet = this.dbContext.Set<TEntity>();
        }

        /// <summary>
        /// The find.
        /// </summary>
        /// <param name="keyValues">
        /// The key values.
        /// </param>
        /// <returns>
        /// The <see cref="TEntity"/>.
        /// </returns>
        public TEntity Find(params object[] keyValues)
        {
            return this.dbSet.Find(keyValues);
        }

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="TEntity"/>.
        /// </returns>
        public virtual TEntity Insert(TEntity entity)
        {
            this.dbSet.Add(entity);
            return entity;
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="TEntity"/>.
        /// </returns>
        public virtual TEntity Update(TEntity entity)
        {
            this.dbSet.Attach(entity);
            this.SetModified(entity);

            return entity;
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public void Delete(Guid id)
        {
            var entity = this.Find(id);
            this.Delete(entity);
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public virtual void Delete(TEntity entity)
        {
            if (this.dbContext.Entry(entity).State == EntityState.Detached)
            {
                this.dbSet.Attach(entity);
            }

            this.dbSet.Remove(entity);
        }

        /// <summary>
        /// Get all the elements
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<TEntity> Queryable()
        {
            return this.dbSet.AsQueryable();
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
        public virtual async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await this.FindAsync(CancellationToken.None, keyValues);
        }

        /// <summary>
        /// The find async.
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <param name="keyValues">
        /// The key values.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, object[] keyValues)
        {
            return await this.dbSet.FindAsync(cancellationToken, keyValues);
        }

        /// <summary>
        /// The delete async.
        /// </summary>
        /// <param name="keyValues">
        /// The key Values.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<bool> DeleteAsync(params object[] keyValues)
        {
            return await this.DeleteAsync(CancellationToken.None, keyValues);
        }

        /// <summary>
        /// The delete async.
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <param name="keyValues">
        /// The key Values.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            var entity = await this.FindAsync(cancellationToken, keyValues);

            if (entity == null)
            {
                return false;
            }

            this.Delete(entity);

            return true;
        }

        /// <summary>
        /// The single async.
        /// </summary>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <param name="paths">
        /// The paths.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate, IEnumerable<Expression<Func<TEntity, object>>> paths)
        {
            IQueryable<TEntity> aux = this.dbSet;
            if (paths != null)
            {
                foreach (var path in paths)
                {
                    aux = aux.Include(path);
                }
            }

            return await aux.SingleOrDefaultAsync(predicate);
        }

        /// <summary>
        /// The set modified.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected virtual void SetModified(TEntity entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
