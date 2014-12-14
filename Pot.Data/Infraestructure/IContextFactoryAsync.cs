namespace Pot.Data.Infraestructure
{
    using System;

    public interface IContextFactoryAsync : IDisposable
    {
        IUnitOfWorkAsync UnitOfWorkAsync { get; }

        IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : class;
    }
}
