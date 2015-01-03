using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pot.Web.Api.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using System.Web.Http.Results;

    using Pot.Data;
    using Pot.Data.Infraestructure;
    using Pot.Data.Model;
    using Pot.Web.Api.Model;

    public class ProjectsController : BaseController<Project, ProjectResource>
    {

          /// <summary>
        /// The uri base for use in test.
        /// </summary>
        internal const string UriBase = "api/Projects/";

        public ProjectsController(IContextFactoryAsync projectsFactory)
            : base(projectsFactory, projectsFactory.GetRepositoryAsync<Project>(), new ProjectResource())
        {
        }

        /// GET: api/Projects
        /// <summary>
        /// The get Projects.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public async Task<IQueryable<ProjectResource>> GetProjects()
        {
            return await this.GetAll();
        }

        /// GET: api/Projects/5
        /// <summary>
        /// The get projects.
        /// </summary> 
        /// <param name="id">
        /// The projects id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <remarks> 
        /// Returns code:
        ///     if found:       Ok Response         (200)  
        ///     If not Found:   Not Found Response  (404) 
        /// </remarks>
        [ResponseType(typeof(ProjectResource))]
        public async Task<IHttpActionResult> GetProject(Guid id)
        {
            return await this.Get(w => w.ProjectId == id, null);
        }

        /// PUT: api/Projects/5
        /// <summary>
        /// The put.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="projectsResource">
        /// The projects Resource.
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
        public async Task<IHttpActionResult> PutProject(Guid id, ProjectResource projectsResource)
        {
            if (id != projectsResource.ProjectId)
            {
                return this.BadRequest();
            }

            return await this.Put(p => p.ProjectId == id, projectsResource);
        }

        /// POST: api/Projects
        /// <summary>
        /// Creates a projects.
        /// </summary>
        /// <param name="project">
        /// The projects.
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
        [ResponseType(typeof(ProjectResource))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> PostProject(ProjectResource project)
        {
            if (project.ProjectId == Guid.Empty)
            {
                project.ProjectId = Guid.NewGuid();
            }

            var result = await this.Post(project.ProjectId, project);

            var response = result as OkNegotiatedContentResult<Project>;
            if (response != null)
            {
                var projectsInserted = await this.BaseRepository.SingleAsync(w => w.ProjectId == response.Content.ProjectId, null);
                return this.CreatedAtRoute(WebApiConfig.DefaultApi, new { id = projectsInserted.ProjectId }, project.MapFrom(projectsInserted));
            }

            return result;
        }

        /// DELETE: api/Projects/5
        /// <summary>
        /// Delete Project
        /// </summary>
        /// <param name="id">
        /// Project id
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [ResponseType(typeof(ProjectResource))]
        public async Task<IHttpActionResult> DeleteProject(Guid id)
        {
            return await this.Delete(p => p.ProjectId == id);
        }
    }
}