namespace Pot.Data.Infraestructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IRepositoryAsync<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        Task<TEntity> FindAsync(params object[] keyValues);
       
        Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
       
        Task<bool> DeleteAsync(params object[] keyValues);
      
        Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues);

        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate, IEnumerable<Expression<Func<TEntity, object>>> paths);
    }
}