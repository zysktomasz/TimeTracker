using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.Domain.Entities;
using TimeTracker.Services.DTO.Activity;
using TimeTracker.Services.DTO.Project;

namespace TimeTracker.Services.AutoMapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Activity, ActivityDto>();
            CreateMap<Activity, ActivityStartReturnDto>();
            CreateMap<Project, ProjectDto>();
        }
    }
}
