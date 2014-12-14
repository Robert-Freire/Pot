namespace Pot.Data.Infraestructure
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IUnitOfWorkAsync : IUnitOfWork
    {
        Task<SaveStatus> SaveChangesAsync();

        Task<SaveStatus> SaveChangesAsync(CancellationToken cancellationToken);
    }
}