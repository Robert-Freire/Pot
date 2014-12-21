
namespace Pot.Data
{
    using Pot.Data.Infraestructure;
    using Pot.Data.Model;

    /// <summary>
    /// The CustomerFactory interface.
    /// </summary>
    public interface IUserFactory : IContextFactoryAsync
    {
        /// <summary>
        /// Gets the customer repository.
        /// </summary>
        IRepositoryAsync<User> UsersRepository { get; }
    }
}