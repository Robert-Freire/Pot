namespace Pot.Web.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using System.Web.Http.Results;

    using Pot.Data.Model;
    using Pot.Data.SQLServer;
    using Pot.Web.Api.Model;

    public class UsersController :  BaseController<User, UserResource>
    {
  
        /// <summary>
        /// The uri base for use in test.
        /// </summary>
        internal const string UriBase = "api/Users/";

        public UsersController(IUserFactory userFactory)
            : base(userFactory, userFactory.UsersRepository, new UserResource())
        {
            //this.authFactory = authFactory;
            //this.authUserRepository = authFactory.AuthUserRepository;
            //this.authUnitOfWork = authFactory.UnitOfWorkAsync;
        }

        /// GET: api/Customers
        /// <summary>
        /// The get customers.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public async Task<IQueryable<UserResource>> GetUsers()
        {
            return await this.GetAll();
        }

        /// GET: api/Customers/5
        /// <summary>
        /// The get customer.
        /// </summary> 
        /// <param name="id">
        /// The customer id.
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
            return await this.Get(w => w.UserId == id, CustomerIncludes.All.GetIncludes());
        }

        /// PUT: api/Customers/5
        /// <summary>
        /// The put.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="customerResource">
        /// The customer Resource.
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
        public async Task<IHttpActionResult> PutCustomer(Guid id, CustomerResource customerResource)
        {
            if (id != customerResource.idCustomer)
            {
                return this.BadRequest();
            }

            return await this.Put(id, customerResource);
        }

        /// POST: api/Customers
        /// <summary>
        /// Creates a customer.
        /// </summary>
        /// <param name="customer">
        /// The customer.
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
        [ResponseType(typeof(CustomerResource))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> PostCustomer(CustomerResource customer)
        {
            var createdUser = (this.User == null) || (this.User.Identity == null)
                              || string.IsNullOrWhiteSpace(this.User.Identity.Name)
                ? customer.name
                : this.User.Identity.Name;

            var result = await this.Post(customer.idCustomer, customer, createdUser);

            var response = result as OkNegotiatedContentResult<Customer>;
            if (response != null)
            {
                // TODO To translate the logic of create users and authorithations at a new service
                await this.authUserRepository.Insert(customer.MapTo());
                await this.authUnitOfWork.SaveChangesAsync();

                var customerInserted = await this.BaseRepository.SingleAsync(w => w.IdCustomer == response.Content.IdCustomer, CustomerIncludes.All.GetIncludes());
                return this.CreatedAtRoute(WebApiConfig.DefaultApi, new { id = customerInserted.IdCustomer }, customer.MapFrom(customerInserted));
            }

            return result;
        }

        /// DELETE: api/Customers/5
        /// <summary>
        /// Delete Customer
        /// </summary>
        /// <param name="id">
        /// Customer id
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [ResponseType(typeof(CustomerResource))]
        public async Task<IHttpActionResult> DeleteCustomer(Guid id)
        {
            return await this.Delete(id);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.authUnitOfWork.Dispose();
                this.authFactory.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
    }
}