namespace Pot.Web.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Pot.Data.Infraestructure;
    using Pot.Web.Api.Model;

    /// <summary>
    /// The base controller.
    /// </summary>
    /// <typeparam name="T">
    /// Main entity related to resource
    /// </typeparam>
    /// <typeparam name="TResource">
    /// Resource exposed
    /// </typeparam>
    public abstract class BaseController<T, TResource> : ApiController
        where T : class
        where TResource : IMapResource<T, TResource>
    {
        private readonly IRepositoryAsync<T> baseRepository;

        private readonly IContextFactoryAsync contextFactory;

        private readonly IUnitOfWorkAsync unitOfWorkAsync;

        private readonly TResource mapResource;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController{T,TResource}"/> class.
        /// </summary>
        /// <param name="contextFactory">
        /// The context factory.
        /// </param>
        /// <param name="baseRepository">
        /// The base repository.
        /// </param>
        /// <param name="mapResource">
        /// The resource.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Bad parameters
        /// </exception>
        protected BaseController(IContextFactoryAsync contextFactory, IRepositoryAsync<T> baseRepository, TResource mapResource)
        {
            if (contextFactory == null)
            {
                throw new ArgumentNullException("contextFactory");
            }

            this.contextFactory = contextFactory;
            this.baseRepository = baseRepository;
            this.mapResource = mapResource;
            this.unitOfWorkAsync = this.contextFactory.UnitOfWorkAsync;
        }

        /// <summary>
        /// Gets the base repository.
        /// </summary>
        protected IRepositoryAsync<T> BaseRepository
        {
            get
            {
                return this.baseRepository;
            }
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected async Task<IQueryable<TResource>> GetAll()
        {
            var entities = await this.baseRepository.Queryable().ToListAsync();
            return entities.AsQueryable().Select(entity => this.mapResource.MapFrom(entity));


            // TODO revisar el posible cambio a proyecciones
            // return entities.AsQueryable().Project().To<TResource>();
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <param name="includes">
        /// The includes.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <remarks> 
        /// Returns code:
        ///     if found:       Ok Response         (200)  
        ///     If not Found:   Not Found Response  (404) 
        /// </remarks>
        protected async Task<IHttpActionResult> Get(Expression<Func<T, bool>> predicate, IEnumerable<Expression<Func<T, object>>> includes)
        {
            var entity = await this.baseRepository.SingleAsync(predicate, includes);
            if (entity == null)
            {
                return this.NotFound();
            }

            return this.Ok(this.mapResource.MapFrom(entity));
        }

        protected async Task<IQueryable<TResource>> GetFiltered(Expression<Func<T, bool>> predicate, IEnumerable<Expression<Func<T, object>>> includes)
        {
            var queryable = this.baseRepository.Queryable();
            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            if (includes != null)
            {
                queryable = includes.Aggregate(queryable, (current, path) => current.Include(path));
            }

            var entities = await queryable.ToListAsync();
            return entities.AsQueryable().Select(entity => this.mapResource.MapFrom(entity));

            // TODO revisar el posible cambio a proyecciones
            // return entities.AsQueryable().Project().To<TResource>();
        }

        ///// <summary>
        ///// The put.
        ///// </summary>
        ///// <param name="id">
        ///// The id.
        ///// </param>
        ///// <param name="resourceToUpdate">
        ///// The resource To Update.
        ///// </param>
        ///// <returns>
        ///// The <see cref="Task"/>.
        ///// </returns>
        ///// <remarks>
        ///// Returns code:
        /////     if data is incorrect:                                   Bad Request Response    (400)  
        /////     if data fails validations:                              Bad Request Response    (400)  
        /////     If data not exists:                                     Not Found Response      (404)       
        /////     If data has already been updated for other process:     Conflict Response       (409)       
        /////     If all is ok:                                           No Content Response     (204)                
        ///// </remarks>
        //protected async Task<IHttpActionResult> Put(Guid id, TResource resourceToUpdate)
        //{
        //    if (!this.ModelState.IsValid)
        //    {
        //        return this.BadRequest(this.ModelState);
        //    }

        //    var entityToUpdate = await this.baseRepository.FindAsync(id).WithCurrentCulture();

        //    if (entityToUpdate == null)
        //    {
        //        return this.NotFound();
        //    }

        //    entityToUpdate = resourceToUpdate.MapTo(entityToUpdate);
        //    this.baseRepository.Update(entityToUpdate);
        //    try
        //    {
        //        var result = await this.unitOfWorkAsync.SaveChangesAsync().WithCurrentCulture();
        //        if (!result.IsValid)
        //        {
        //            var err = new ValidationException(result.Errors.ToString());
        //            ErrorLog.LogError(err, result.Errors.ToString());

        //            return ApiControllerExtension.ValidationsErrorResult(this, result.Errors.ToList());
        //        }
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        ErrorLog.LogError(ex);

        //        return this.StatusCode(HttpStatusCode.Conflict);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.LogError(ex);
        //    }

        //    return this.StatusCode(HttpStatusCode.NoContent);
        //}

        /// <summary>
        /// The put.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="predicate">
        /// The predicate
        /// </param>
        /// <param name="resourceToUpdate">
        /// The resource To Update.
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
        ///     If all is ok:                                           No Content Response     (204)                
        /// </remarks>
        protected async Task<IHttpActionResult> Put(Expression<Func<T, bool>> predicate, TResource resourceToUpdate)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var entityToUpdate = await this.baseRepository.Queryable().SingleOrDefaultAsync(predicate).WithCurrentCulture();

            if (entityToUpdate == null)
            {
                return this.NotFound();
            }

            entityToUpdate = resourceToUpdate.MapTo(entityToUpdate);
            this.baseRepository.Update(entityToUpdate);
            try
            {
                var result = await this.unitOfWorkAsync.SaveChangesAsync().WithCurrentCulture();
                if (!result.IsValid)
                {
                    var err = new ValidationException(result.Errors.ToString());
                    ErrorLog.LogError(err, result.Errors.ToString());

                    return ApiControllerExtension.ValidationsErrorResult(this, result.Errors.ToList());
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ErrorLog.LogError(ex);

                return this.StatusCode(HttpStatusCode.Conflict);
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
            }

            return this.StatusCode(HttpStatusCode.NoContent);
        }
        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="resourceToInsert">
        /// The resource to insert.
        /// </param>
        /// <param name="createdUser">
        /// The created user.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <remarks>
        /// Returns code:
        ///     if data is incorrect:                                   Bad Request Response    (400)  
        ///     if data fails validations:                              Bad Request Response    (400)  
        ///     If data has already been updated for other process:     Conflict Response       (409)       
        ///     If all is ok:                                           Ok Response             (200)          
        /// </remarks>
        protected async Task<IHttpActionResult> Post(TResource resourceToInsert, Guid? id = null, Expression<Func<T, bool>> predicate = null)
        {
            if (!this.ModelState.IsValid)
            {
                return null;
            }

            var entityToInsert = resourceToInsert.MapTo();

            if (entityToInsert == null)
            {
                return this.BadRequest();
            }

            entityToInsert = this.baseRepository.Insert(entityToInsert);

            return await this.SavePost(entityToInsert, id, predicate);
        }

        /// <summary>
        /// The save post.
        /// </summary>
        /// <param name="entityToInsert">
        /// The entity to insert.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected async Task<IHttpActionResult> SavePost(T entityToInsert, Guid? id = null, Expression<Func<T, bool>> predicate = null)
        {
            try
            {
                var result = await this.unitOfWorkAsync.SaveChangesAsync().WithCurrentCulture();
                if (!result.IsValid)
                {
                    var err = new ValidationException(result.Errors.ToString());
                    ErrorLog.LogError(err, result.Errors.ToString());

                    return this.ValidationsErrorResult(result.Errors.ToList());
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ErrorLog.LogError(ex);

                return this.StatusCode(HttpStatusCode.Conflict);
            }
            catch (DbUpdateException ex)
            {
                ErrorLog.LogError(ex);

                T entityExists = null;
                if (id != null)
                {
                    entityExists = this.baseRepository.Find(id);
                }
                else if (predicate != null)
                {
                    entityExists = this.baseRepository.Queryable().SingleOrDefault(predicate);
                }

                if (entityExists != null)
                {
                    return this.Conflict();
                }

                throw;
            }

            return this.Ok(entityToInsert);
        }


        ///// <summary>
        ///// The delete.
        ///// </summary>
        ///// <param name="id">
        ///// The id.
        ///// </param>
        ///// <returns>
        ///// The <see cref="Task"/>.
        ///// </returns>
        ///// <remarks>
        ///// Returns code:
        /////     If data not exists:                                     Not Found Response      (404)       
        /////     if data fails validations:                              Bad Request Response    (400)  
        /////     If all is ok:                                           Ok Response             (200)                
        ///// </remarks>
        //protected async Task<IHttpActionResult> Delete(Guid id)
        //{
        //    var entity = await this.baseRepository.FindAsync(id).WithCurrentCulture();
        //    if (entity == null)
        //    {
        //        return this.NotFound();
        //    }

        //    this.baseRepository.Delete(entity);

        //    var result = await this.unitOfWorkAsync.SaveChangesAsync().WithCurrentCulture();
        //    if (!result.IsValid)
        //    {
        //        var err = new ValidationException(result.Errors.ToString());
        //        ErrorLog.LogError(err, result.Errors.ToString());

        //        return ApiControllerExtension.ValidationsErrorResult(this, result.Errors.ToList());
        //    }

        //    return this.Ok(this.mapResource.MapFrom(entity));
        //}

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="predicate">
        /// The predicate 
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <remarks>
        /// Returns code:
        ///     If data not exists:                                     Not Found Response      (404)       
        ///     if data fails validations:                              Bad Request Response    (400)  
        ///     If all is ok:                                           Ok Response             (200)                
        /// </remarks>
        protected async Task<IHttpActionResult> Delete(Expression<Func<T, bool>> predicate)
        {
            var entity = await this.baseRepository.Queryable().FirstOrDefaultAsync(predicate).WithCurrentCulture();
            if (entity == null)
            {
                return this.NotFound();
            }

            this.baseRepository.Delete(entity);

            var result = await this.unitOfWorkAsync.SaveChangesAsync().WithCurrentCulture();
            if (!result.IsValid)
            {
                var err = new ValidationException(result.Errors.ToString());
                ErrorLog.LogError(err, result.Errors.ToString());

                return ApiControllerExtension.ValidationsErrorResult(this, result.Errors.ToList());
            }

            return this.Ok(this.mapResource.MapFrom(entity));
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
                this.unitOfWorkAsync.Dispose();
                this.contextFactory.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}