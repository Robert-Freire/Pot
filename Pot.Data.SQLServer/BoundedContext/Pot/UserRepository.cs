namespace Pot.Data.SQLServer
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using Pot.Data.Model;
    using Pot.Data.SQLServer.Utis;

    public class UserRepository : Repository<User>
    {
        private readonly UserManager<User> userManager;
        public UserRepository(PotDbContext potDbContext)
            : base(potDbContext)
        {
            this.userManager = new UserManager<User>(new UserStore<User>(potDbContext));
        }

        public override User Insert(User user)
        {
            this.userManager.Create(user, user.Password);
            return user;
        }
    }
}
