using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pot.Web.Api.Model
{
    using AutoMapper;

    using Pot.Data.Model;

    public class ProjectResource : IMapResource<Project, ProjectResource>
    {

        public Guid ProjectId { get; set; }


        public string Name { get; set; }


        public byte[] Version { get; set; }

        public ProjectResource MapFrom(Project entity)
        {
            return entity == null ? null : Mapper.Map<ProjectResource>(entity);
        }

        public Project MapTo()
        {
            return Mapper.Map<Project>(this);
        }

        public Project MapTo(Project entity)
        {
            return Mapper.Map(this, entity);
        }

        public UserResource MapFrom(User entity)
        {
            return entity == null ? null : Mapper.Map<UserResource>(entity);
        }

        internal static void InitializeMappings()
        {
            Mapper.CreateMap<Project, ProjectResource>();

            Mapper.CreateMap<ProjectResource, Project>()
                .ForMember(u => u.Expenses, opt => opt.Ignore())
                .ForMember(u => u.ProjectUsers, opt => opt.Ignore());
        }
    }
}