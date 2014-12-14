namespace Pot.Data.Infraestructure
{
    using System;
    using System.Linq;

    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Find(params object[] keyValues);
       
        TEntity Insert(TEntity entity);
     
        TEntity Update(TEntity entity);
      
        void Delete(Guid id);
       
        void Delete(TEntity entity);
    
        IQueryable<TEntity> Queryable();
    }
}