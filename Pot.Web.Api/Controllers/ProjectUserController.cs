// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TranslatorMotherTonguesController.cs" company="Nova Language Services">
//   Copyright © Nova Language Services 2014
// </copyright>
// <summary>
//   The customers controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Pot.Web.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Pot.Data.Infraestructure;
    using Pot.Data.Model;
    using Pot.Web.Api.Model;

    /// <summary>
    /// The customers controller.
    /// </summary>
    [Authorize]
    internal class ProjectUserController : BaseController<ProjectUser, UserResource>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TranslatorMotherTonguesController"/> class. 
        /// </summary>
        /// <param name="translatorFactory">
        /// The translator Factory.
        /// </param>
        /// <param name="projectUserFactory"></param>
        /// <exception cref="ArgumentNullException">
        /// CustomerFactory is not informed
        /// </exception>
        internal ProjectUserController(IContextFactoryAsync projectUserFactory)
            : base(projectUserFactory, projectUserFactory.GetRepositoryAsync<ProjectUser>(), new UserResource())
        {
        }

        /// <summary>
        /// The get filtered.
        /// </summary>
        /// <param name="idProject">
        /// The id project.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        internal async Task<IQueryable<UserResource>> GetFiltered(Guid projectId)
        {
            return await base.GetFiltered(p => p.ProjectId == projectId, null);
        }

        /// <summary>
        /// The get.
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
        ///     if found:       Ok Response         (200)  
        ///     If not Found:   Not Found Response  (404) 
        /// </remarks>
        [ResponseType(typeof(UserResource))]
        internal async Task<IHttpActionResult> Get(Guid projectId, Guid userId)
        {
            var includes = GetIncludes();

            return await this.Get(w => w.ProjectId == projectId && w.UserId == userId, includes);
        }

        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="idTranslator">
        /// The id translator.
        /// </param>
        /// <param name="translatorMotherTongue">
        /// The translator mother tongue.
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
        internal async Task<IHttpActionResult> Post(Guid idProject, UserResource projectUser)
        {
            projectUser.ProjectId = idProject;
            return
                await
                    base.Post(projectUser, predicate: p => p.ProjectId == projectUser.ProjectId && p.UserId == projectUser.UserId);
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="idTranslator">
        /// The id translator.
        /// </param>
        /// <param name="idLanguage">
        /// The id language.
        /// </param>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <remarks>
        /// Returns code:
        ///     If data not exists:                                     Not Found Response      (404)       
        ///     if data fails validations:                              Bad Request Response    (400)  
        ///     If all is ok:                                           Ok Response             (200)                
        /// </remarks>/// 
        [ResponseType(typeof(UserResource))]
        internal async Task<IHttpActionResult> Delete(Guid idProject, Guid idUser)
        {
            return await base.Delete(p => p.ProjectId == idProject && p.UserId == idUser);
        }

        /// <summary>
        /// The get includes.
        /// </summary>
        /// <returns>
        /// The <see cref="TranslatorMotherTongue"/>.
        /// </returns>
        private static IEnumerable<Expression<Func<ProjectUser, object>>> GetIncludes()
        {
            return new List<Expression<Func<ProjectUser, object>>> { c => c.Project, c => c.User };
        }
    }
}