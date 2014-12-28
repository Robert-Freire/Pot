namespace Pot.Data.SQLServer
{
    using System.Data.Entity;

    using Pot.Data;
    using Pot.Data.Infraestructure;
    using Pot.Data.Model;
    using Pot.Data.SQLServer.Utis;


    public class UserFactory : ContextFactory, IUserFactory
    {
        public UserFactory(PotDbContext dbContext)
            : base(dbContext)
        {
        }

      public virtual IRepositoryAsync<User> UsersRepository
        {
            get
            {
                return new UserRepository(PotDbContext);
            }

        }
    }
}
