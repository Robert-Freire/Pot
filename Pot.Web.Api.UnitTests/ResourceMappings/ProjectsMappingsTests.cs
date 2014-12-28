// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectsMappingsTests.cs" company="Nova Language Services">
//   Copyright © Nova Language Services 2014
// </copyright>
// <summary>
//   The projects mappings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Pot.Web.Api.Unit.Tests
{
    using System;

    using FluentAssertions;

    using NUnit.Framework;

    using Pot.Data.Model;
    using Pot.Web.Api.Model;
    using Pot.Web.Api.Tests.Utils;

    [TestFixture]
    public class ProjectsMappingsTests
    {
        public ProjectsMappingsTests()
        {
            AutomapperConfig.Initialize();
        }

        [Test]
        public void Projects_MapToResource_ResourceIsMapped()
        {
            // Arrange
            var project = new Project { Name = "Project test", ProjectId = Guid.NewGuid() };

            // Action
            var projectResource = new ProjectResource().MapFrom(project);

            // Assert
            projectResource.ShouldBeEquivalentToProject(project);
        }


        [Test]
        public void ProjectsResource_MapToProject_ProjectIsmapped()
        {
            // Arrange
            var projectResource = new ProjectResource { Name = "Project test" };

            // Act
            var project = projectResource.MapTo();

            // Assert
            project.ShouldBeEquivalentToProject(projectResource);
        }

        /// <summary>
        /// Check the map from project resource to existing project
        /// </summary>
        [Test]
        public void ProjectsResource_MapToExistingProject_ProjectIsmapped()
        {
            // Arrange
            var project = new Project { ProjectId = Guid.NewGuid(), Name = "Original name" };
            var projectResource = new ProjectResource { ProjectId = Guid.NewGuid(), Name = "Modified name" };

            // Action
            var modifiedProject = projectResource.MapTo(project);

            // Assert
            modifiedProject.ShouldBeEquivalentToProject(projectResource);
            modifiedProject.GetHashCode().Should().Be(project.GetHashCode());
        }
    }
}
