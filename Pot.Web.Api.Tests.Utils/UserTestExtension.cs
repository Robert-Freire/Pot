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

    internal static class UserTestExtension
    {
        private static int cont = 0;

        internal static UserResource GetDefault()
        {
            cont++;
            return new UserResource
            {
                UserName = "Customer" + cont,
                UserId = Guid.NewGuid(),
                Password = "Customer" + cont,
                Email = "newMail@mail.com"
            };
        }

        internal static UserResource GetUserResource(this HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<UserResource>(response.Content.ReadAsStringAsync().Result);
        }

        internal static void ShouldBeEquivalentToUser(this User user, UserResource userResource)
        {
           // user.ShouldBeEquivalentTo(userResource, opt => opt.Excluding(e => e.Version).ExcludingMissingProperties());
            user.ShouldBeEquivalentTo(userResource, opt => opt.ExcludingMissingProperties());
        }

        internal static void ShouldBeEquivalentToUser(this UserResource userResource, User user)
        {
            //userResource.ShouldBeEquivalentTo(
            //    user,
            //    opt => opt.ExcludingMissingProperties().ExcludingNestedObjects().Excluding(e => e.Email).Excluding(e => e.Version));
            userResource.ShouldBeEquivalentTo(
                          user,
                          opt => opt.ExcludingMissingProperties().ExcludingNestedObjects());
        }

        internal static void ShouldBeEquivalentToProjectUser(this UserResource userResource, ProjectUser projectUser)
        {
            userResource.ShouldBeEquivalentTo(
                          projectUser,
                          opt => opt.ExcludingMissingProperties().ExcludingNestedObjects());
        }

        internal static void ShouldBeEquivalent(this IHttpActionResult result, User user)
        {
            result.Should().BeOfType(typeof(OkNegotiatedContentResult<UserResource>));
            var userResource = ((OkNegotiatedContentResult<UserResource>)result).Content;
            userResource.ShouldBeEquivalentToUser(user);
        }

        internal static void ShouldBeEquivalentTo(this HttpResponseMessage response, User user)
        {
            var userResource = GetUserResource(response);
            userResource.ShouldBeEquivalentToUser(user);
        }

        internal static void ShouldBeEquivalentTo(this HttpResponseMessage response, UserResource user)
        {
            var userResource = GetUserResource(response);
            userResource.ShouldBeEquivalentTo(user, opt => opt.Excluding(v => v.Password));
//            userResource.ShouldBeEquivalentTo(user, opt=> opt.Excluding(f => f.Version));
        }
    }
}
