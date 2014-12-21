namespace Pot.Web.Api.Unit.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Pot.Data.Model;
    using Pot.Web.Api.Model;

    internal static class UserTestExtension
    {
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
    }
}
