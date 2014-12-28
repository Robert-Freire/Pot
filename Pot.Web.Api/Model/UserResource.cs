namespace Pot.Web.Api.Model
{
    using System;

    using AutoMapper;

    using Pot.Data.Model;

    public class UserResource : IMapResource<User, UserResource>
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }


        public string Email { get; set; }

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

            Mapper.CreateMap<UserResource, User>()
                .ForMember(u => u.ProjectUsers, opt => opt.Ignore())
                .ForMember(u => u.Expenses, opt => opt.Ignore())
                .ForMember(u => u.EmailConfirmed, opt => opt.Ignore())
                .ForMember(u => u.PasswordHash, opt => opt.Ignore())
                .ForMember(u => u.SecurityStamp, opt => opt.Ignore())
                .ForMember(u => u.PhoneNumber, opt => opt.Ignore())
                .ForMember(u => u.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(u => u.TwoFactorEnabled, opt => opt.Ignore())
                .ForMember(u => u.LockoutEndDateUtc, opt => opt.Ignore())
                .ForMember(u => u.LockoutEnabled, opt => opt.Ignore())
                .ForMember(u => u.AccessFailedCount, opt => opt.Ignore())
                .ForMember(u => u.Roles, opt => opt.Ignore())
                .ForMember(u => u.Claims, opt => opt.Ignore())
                .ForMember(u => u.Logins, opt => opt.Ignore())
                .ForMember(u => u.Id, opt => opt.Ignore());
        }
    }
}