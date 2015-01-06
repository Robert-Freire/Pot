namespace Pot.Web.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using System.Web.Http.Results;

    using Pot.Data;
    using Pot.Data.Model;
    using Pot.Data.SQLServer;
    using Pot.Web.Api.Model;

    public class UsersController : BaseController<User, UserResource>
    {
        /// <summary>
        /// The uri base for use in test.
        /// </summary>
        internal const string UriBase = "api/Users/";

        public UsersController(IUserFactory userFactory)
            : base(userFactory, userFactory.UsersRepository, new UserResource())
        {
        }

        /// GET: api/Users
        /// <summary>
        /// The get Users.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public async Task<IQueryable<UserResource>> GetUsers()
        {
            return await this.GetAll();
        }

        /// GET: api/Users/5
        /// <summary>
        /// The get users.
        /// </summary> 
        /// <param name="id">
        /// The users id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <remarks> 
        /// Returns code:
        ///     if found:       Ok Response         (200)  
        ///     If not Found:   Not Found Response  (404) 
        /// </remarks>
        [ResponseType(typeof(UserResource))]
        public async Task<IHttpActionResult> GetUser(Guid id)
        {
            return await this.Get(w => w.UserId == id, null);
        }

        /// PUT: api/Users/5
        /// <summary>
        /// The put.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="usersResource">
        /// The users Resource.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <remarks>
        /// Returns code:
        ///     if data is incorrect:                                   Bad Request Response    (400)  
        ///     if data fails validations:                              Bad Request Response    (400)  
        ///     If data not exists:                                     Not Found Response      (404)       
        ///     If data has already been updated for other process:     Conflict Response       (409)       
        ///     If all is ok:                                           No Content              (204)                
        /// </remarks>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(Guid id, UserResource usersResource)
        {
            if (id != usersResource.UserId)
            {
                return this.BadRequest();
            }

            return await this.Put(p => p.UserId == id, usersResource);
        }

        /// POST: api/Users
        /// <summary>
        /// Creates a users.
        /// </summary>
        /// <param name="users">
        /// The users.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <remarks>
        /// Returns code:
        ///     if data is incorrect:                                   Bad Request Response    (400)  
        ///     if data fails validations:                              Bad Request Response    (400)  
        ///     If data has already been updated for other process:     Conflict Response       (409)       
        ///     If all is ok:                                           ok Response             (200)          
        /// </remarks>
        [ResponseType(typeof(UserResource))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> PostUser(UserResource users)
        {
            var result = await this.Post(users, users.UserId);

            var response = result as OkNegotiatedContentResult<User>;
            if (response != null)
            {
                var usersInserted = await this.BaseRepository.SingleAsync(w => w.UserId == response.Content.UserId, null);
                return this.CreatedAtRoute(WebApiConfig.DefaultApi, new { id = usersInserted.UserId }, users.MapFrom(usersInserted));
            }

            return result;
        }

        /// DELETE: api/Users/5
        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="id">
        /// User id
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [ResponseType(typeof(UserResource))]
        public async Task<IHttpActionResult> DeleteUser(Guid id)
        {
            return await this.Delete(p => p.UserId == id);
        }
    }
}