namespace Pot.Data.SQLServer
{
    using System.Data.Entity;

    using Pot.Data;
    using Pot.Data.Infraestructure;
    using Pot.Data.Model;
    using Pot.Data.SQLServer.Utis;


    public class UserFactory : ContextFactory, IUserFactory
    {
        private PotDbContext potDbContext;
        public UserFactory(PotDbContext dbContext)
            : base(dbContext)
        {
            potDbContext = dbContext;
        }

      public virtual IRepositoryAsync<User> UsersRepository
        {
            get
            {
                return new UserRepository(potDbContext);
            }

        }
    }
}
