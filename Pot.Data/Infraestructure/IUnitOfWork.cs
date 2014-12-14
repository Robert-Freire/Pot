namespace Pot.Data.Infraestructure
{
    using System;

    public interface IUnitOfWork : IDisposable
    {
       SaveStatus SaveChanges();

       void BeginTransaction();

        bool Commit();

        void Rollback();
    }
}
