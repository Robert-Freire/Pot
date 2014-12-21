using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pot.Data.BoundedContext.Pot
{
    using global::Pot.Data.Infraestructure;
    using global::Pot.Data.Model;

    public interface IUserRepository : IRepositoryAsync<User>
    {
    }
}
