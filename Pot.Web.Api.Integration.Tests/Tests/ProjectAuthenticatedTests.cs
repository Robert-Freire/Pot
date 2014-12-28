// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectAuthenticatedTests.cs" company="Nova Language Services">
//   Copyright © Nova Language Services 2014
// </copyright>
// <summary>
//   The project OWIN authenticated tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Pot.Web.Api.Integration.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Newtonsoft.Json;

    using NUnit.Framework;

    using Pot.Data.Infraestructure;
    using Pot.Data.Model;
    using Pot.Data.SQLServer;
    using Pot.Web.Api.Controllers;
    using Pot.Web.Api.Integration.Tests.Utils;
    using Pot.Web.Api.Model;
    using Pot.Web.Api.Tests.Utils;

    /// <summary>
    /// The project OWIN authenticated tests.
    /// </summary>
    [TestFixture]
    public class ProjectAuthenticatedTests : BaseAuthenticatedTests
    {
        /// <summary>
        /// The authorization repository.
        /// </summary>
        //private readonly IRepositoryAsync<Project> projectRepository;

        /// <summary>
        /// The uri.
        /// </summary>
        private string uri;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectAuthenticatedTests"/> class.
        /// </summary>
    //    public ProjectAuthenticatedTests()
    //    {
    //        var authFactory = new ProjectFactory(new PotDbContext());
    //        this.projectRepository = authFactory.ProjectsRepository;
    //    }

        /// <summary>
        /// Gets the uri.
        /// </summary>
        protected override Uri Uri
        {
            get
            {
                return new Uri(DefaultServerUri + this.uri, UriKind.RelativeOrAbsolute);
            }
        }

        /// <summary>
        /// The get all projects if there are two projects then returns two projects.
        /// </summary>
        [Test]
        public void GetAllProjects_ThereAreAlmosTwoProjects_ReturnsTwoProjectsOrMore()
        {
            this.uri = ProjectsController.UriBase;

            using (var response = this.GetAsync().Result)
            {
                // Assert
                response.Should().NotBeNull();
                var projects = JsonConvert.DeserializeObject<List<ProjectResource>>(response.Content.ReadAsStringAsync().Result);
                projects.Count.Should().BeGreaterOrEqualTo(1);
            }
        }

        /// <summary>
        /// The get project if there are no projects then returns not found.
        /// </summary>
        [Test]
        public void GetProject_ThereAreNoProject_ReturnsNotFound()
        {
            this.uri = ProjectsController.UriBase + Guid.NewGuid();

            using (var response = this.GetAsync().Result)
            {
                // Assert
                response.Should().NotBeNull();
                response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// The get project there are one projects returns project
        /// </summary>
        [Test]
        public void GetProject_ThereAreProject_ReturnsProject()
        {
            this.uri = ProjectsController.UriBase + TestsDb.ProjectDefaultTest.ProjectId;
            using (var response = this.GetAsync().Result)
            {
                // Assert
                response.Should().NotBeNull();
                response.ShouldBeEquivalentTo(TestsDb.ProjectDefaultTest);
            }
        }

        /// <summary>
        /// The update project if the project is correct then returns no data.
        /// </summary>
        [Test]
        public void UpdateProject_ProjectIsCorrect_ReturnsNoData()
        {
            // Arrange
            var projectToModify = this.GetProject(TestsDb.ProjectDefaultTest.ProjectId);
            projectToModify.Name = "Modified Project";

            this.uri = ProjectsController.UriBase + projectToModify.ProjectId;

            // Action
            using (var response = this.PutAsync(projectToModify).Result)
            {
                // Assert Response
                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);

                // Assert DB
                var project = this.GetProject(TestsDb.ProjectDefaultTest.ProjectId);
                project.Name.Should().Be(projectToModify.Name);
            }
        }

        /// <summary>
        /// If one project update a project and another project try to update with older data the second update returns error
        /// </summary>
        [Test]
        public void UpdateProject_ProjectIsAlreadyUpdated_ReturnsConcurrencyError()
        {
            // Arrange
            var projectToModify = this.GetProject(TestsDb.ProjectDefaultTest.ProjectId);
            projectToModify.Name = "Modified Project";

            this.uri = ProjectsController.UriBase + projectToModify.ProjectId;

            // Action
            using (var response = this.PutAsync(projectToModify).Result)
            {
                // Assert Response
                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }

            var olderProject = new ProjectResource().MapFrom(TestsDb.ProjectDefaultTest);
            olderProject.Name = "Concurrent error";

            this.uri = ProjectsController.UriBase + olderProject.ProjectId;

            // Action Try to Update client to older values
            using (var response = this.PutAsync(olderProject).Result)
            {
                // Assert Response
                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            }
        }



        /// <summary>
        /// The addNew project if the project is correct then returns created.
        /// </summary>
        [Test]
        public void InsertProject_ProjectIsCorrect_ReturnsCreated()
        {
            // Arrange
            var projectToAdd = ProjectTestExtension.GetDefault();

            this.uri = ProjectsController.UriBase;

            // Action
            using (var response = this.PostAsync(projectToAdd).Result)
            {
                // Assert Response
                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.Created, "the project with name {0} has to be created but fails for {1}", projectToAdd.Name, response.Content.ReadAsStringAsync().Result);
                response.ShouldBeEquivalentTo(projectToAdd);


                // Assert DB
                var projectSaved = this.GetProject(response.GetProjectResource().ProjectId);
                response.ShouldBeEquivalentTo(projectSaved);
            }
        }

        /// <summary>
        /// The addNew project if the project is exists then returns error.
        /// </summary>
        [Test]
        public void InsertProject_ProjectAlreadyExists_ReturnsError()
        {
            // Arrange
            var projectToAdd = this.GetProject(TestsDb.ProjectDefaultTest.ProjectId);
            projectToAdd.Name = "ProjectAdded";

            this.uri = ProjectsController.UriBase;

            // Action
            using (var response = this.PostAsync(projectToAdd).Result)
            {
                // Assert Response
                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            }
        }

        /// <summary>
        /// The delete project if the project is correct then returns delete.
        /// </summary>
        [Test]
        public void DeleteProject_ProjectIsCorrect_ReturnsDeleted()
        {
            // Arrange
            var projectToAdd = ProjectTestExtension.GetDefault();

            this.uri = ProjectsController.UriBase;
            ProjectResource projectRedirect;

            using (var response = this.PostAsync(projectToAdd).Result)
            {
                response.Should().NotBeNull();
                projectRedirect = response.GetProjectResource();
            }

            this.uri = ProjectsController.UriBase + projectRedirect.ProjectId;

            // Action
            using (var response = this.DeleteAsync(projectRedirect).Result)
            {
                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                var project = response.GetProjectResource();
                project.ProjectId.Should().Be(projectRedirect.ProjectId);

                // AssertBD
                var projectDeleted = this.GetProject(project.ProjectId);
                projectDeleted.Should().BeNull();
            }
        }

        /// <summary>
        /// The get project.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The <see cref="ProjectResource"/>.
        /// </returns>
        private ProjectResource GetProject(Guid projectId)
        {
            this.uri = ProjectsController.UriBase + projectId;
            using (var response = this.GetAsync().Result)
            {
                response.Should().NotBeNull();
                return response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<ProjectResource>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
        }

    }
}
