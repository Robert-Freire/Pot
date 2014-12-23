namespace Pot.Web.Api.Unit.Tests
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
        internal static UserResource GetDefault()
        {

            return new UserResource
            {
                Name = "Customer",
                UserId = Guid.NewGuid(),
                Mail = "newMail@mail.com"
            };
        }

        internal static UserResource GetUserResource(this HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<UserResource>(response.Content.ReadAsStringAsync().Result);
        }

        internal static void ShouldBeEquivalentToUser(this User user, UserResource userResource)
        {
            user.ShouldBeEquivalentTo(userResource, opt => opt.Excluding(e => e.Version).ExcludingMissingProperties());
        }

        internal static void ShouldBeEquivalentToUser(this UserResource userResource, User user)
        {
            userResource.ShouldBeEquivalentTo(
                user,
                opt => opt.ExcludingMissingProperties().ExcludingNestedObjects().Excluding(e => e.Mail).Excluding(e => e.Version));
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
            userResource.ShouldBeEquivalentTo(user, opt=> opt.Excluding(f => f.Version));
        }
    }
}
