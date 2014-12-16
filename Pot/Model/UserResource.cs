namespace Pot.Web.Api.Model
{
    using System;

    using AutoMapper;

    using Pot.Data.Model;

    public class UserResource: IMapResource<User, UserResource>
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }


        public string Mail { get; set; }

        public byte[] Version { get; set; }


        public UserResource MapFrom(User entity)
        {
            return entity == null ? null : Mapper.Map<UserResource>(entity);
        }


        public User MapTo()
        {
            return Mapper.Map<User>(this);
        }

        public User MapTo(User entity)
        {
            return Mapper.Map(this, entity);
        }

        internal static void InitializeMappings()
        {
            Mapper.CreateMap<User, UserResource>();

            Mapper.CreateMap<UserResource, User>();
        }
    }
}