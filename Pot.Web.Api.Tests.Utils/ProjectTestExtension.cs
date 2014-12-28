namespace Pot.Web.Api.Tests.Utils
{
    using System;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Results;

    using FluentAssertions;

    using Newtonsoft.Json;

    using Pot.Data.Model;
    using Pot.Web.Api.Model;

    internal static class ProjectTestExtension
    {
        internal static ProjectResource GetDefault()
        {
            return new ProjectResource
            {
                Name = "Project Tests",
                ProjectId = Guid.NewGuid()
            };
        }

        internal static ProjectResource GetProjectResource(this HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<ProjectResource>(response.Content.ReadAsStringAsync().Result);
        }

        internal static void ShouldBeEquivalentToProject(this Project project, ProjectResource projectResource)
        {
            // user.ShouldBeEquivalentTo(userResource, opt => opt.Excluding(e => e.Version).ExcludingMissingProperties());
            project.ShouldBeEquivalentTo(projectResource, opt => opt.ExcludingMissingProperties().Excluding(p => p.Version));

        }
        internal static void ShouldBeEquivalentToProject(this ProjectResource projectResource, Project project)
        {
            //userResource.ShouldBeEquivalentTo(
            //    user,
            //    opt => opt.ExcludingMissingProperties().ExcludingNestedObjects().Excluding(e => e.Email).Excluding(e => e.Version));
            projectResource.ShouldBeEquivalentTo(
                          project,
                          opt => opt.ExcludingMissingProperties().ExcludingNestedObjects().Excluding( p => p.Version));

        }

        internal static void ShouldBeEquivalent(this IHttpActionResult result, Project project)
        {
            result.Should().BeOfType(typeof(OkNegotiatedContentResult<ProjectResource>));
            var projectResource = ((OkNegotiatedContentResult<ProjectResource>)result).Content;
            projectResource.ShouldBeEquivalentToProject(project);
        }

        internal static void ShouldBeEquivalentTo(this HttpResponseMessage response, Project project)
        {
            var projectResource = GetProjectResource(response);
            projectResource.ShouldBeEquivalentToProject(project);
        }

        internal static void ShouldBeEquivalentTo(this HttpResponseMessage response, ProjectResource project)
        {
            var projectResource = GetProjectResource(response);
            projectResource.ShouldBeEquivalentTo(project, opt => opt.Excluding(p => p.Version));
            //            userResource.ShouldBeEquivalentTo(user, opt=> opt.Excluding(f => f.Version));
        }
    }
}
