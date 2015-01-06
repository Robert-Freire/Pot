namespace Pot.Web.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using System.Web.Http.Results;

    using Pot.Data.Infraestructure;
    using Pot.Data.Model;
    using Pot.Web.Api.Model;

    [RoutePrefix(RoutePrefix)]
    public class ProjectsController : BaseController<Project, ProjectResource>
    {
        /// <summary>
        /// The uri base for use in test.
        /// </summary>
        internal const string UriBase = RoutePrefix + "/";

        private const string RoutePrefix = "api/Projects";

        private ProjectUserController projectUserController;

        public ProjectsController(IContextFactoryAsync projectsFactory)
            : base(projectsFactory, projectsFactory.GetRepositoryAsync<Project>(), new ProjectResource())
        {
            this.projectUserController = new ProjectUserController(projectsFactory);
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

        /// GET: api/Projects/{projectId}
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

        /// PUT: api/Projects/{projectId}
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

            var result = await this.Post(project, project.ProjectId);

            var response = result as OkNegotiatedContentResult<Project>;
            if (response != null)
            {
                var projectsInserted = await this.BaseRepository.SingleAsync(w => w.ProjectId == response.Content.ProjectId, null);
                return this.CreatedAtRoute(WebApiConfig.DefaultApi, new { id = projectsInserted.ProjectId }, project.MapFrom(projectsInserted));
            }

            return result;
        }

        /// DELETE: api/Projects/{projectId}/
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

        /// GET: api/Projects/{idProject}/Users
        /// <summary>
        /// The get translators.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        /// <summary>
        /// The get mother languages.
        /// </summary>
        /// <param name="projectId">
        /// The id translator.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("{projectId}/Users")]
        public async Task<IQueryable<UserResource>> GetProjectUsers(Guid projectId)
        {
            return await this.projectUserController.GetFiltered(projectId);
        }

        /// GET: api/Translators/{idTranslator}/MotherLanguages/{idLanguage}
        /// <summary>
        /// The get mother tongue.
        /// </summary>
        /// <param name="projectId">
        /// The id project.
        /// </param>
        /// <param name="userId">
        /// The id user.
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
        [HttpGet, Route("{projectId}/Users/{userId}")]
        public async Task<IHttpActionResult> GetProjectUser(Guid projectId, Guid userId)
        {
            return await this.projectUserController.Get(projectId, userId);
        }

        /// POST: api/Translators/{idTranslator}/MotherLanguages
        /// <summary>
        /// The post mother language.
        /// </summary>
        /// <param name="idTranslator">
        /// The id translator.
        /// </param>
        /// <param name="language">
        /// The language.
        /// </param>
        /// <returns>
        /// The<see cref="Task"/>.
        /// </returns>
        /// <remarks>
        /// Returns code:
        /// if data is incorrect:                                   Bad Request Response    (400)
        /// if data fails validations:                              Bad Request Response    (400)
        /// If data has already been updated for other process:     Conflict Response       (409)
        /// If all is ok:                                           ok Response             (200)
        /// </remarks>
        [ResponseType(typeof(UserResource))]
        [HttpPost, Route("{idTranslator}/Users")]
        public async Task<IHttpActionResult> PostProjectUser(Guid idTranslator, UserResource language)
        {
            return await this.projectUserController.Post(idTranslator, language);
        }

        /// DELETE: api/Translators/{idTranslator}/MotherLanguages/{idLanguage}
        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="idTranslator">
        /// The id translator.
        /// </param>
        /// <param name="idLanguage">
        /// The id language.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <remarks>
        /// Returns code:
        ///     If data not exists:                                     Not Found Response      (404)       
        ///     if data fails validations:                              Bad Request Response    (400)  
        ///     If all is ok:                                           Ok Response             (200)                
        /// </remarks>/// 
        [ResponseType(typeof(UserResource))]
        [HttpDelete, Route("{idTranslator}/Users/{idLanguage}")]
        public async Task<IHttpActionResult> DeleteProjectUsers(Guid idTranslator, Guid idLanguage)
        {
            return await this.projectUserController.Delete(idTranslator, idLanguage);
        }
    }
}