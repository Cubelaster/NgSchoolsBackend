using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class EducationGroupMApper : Profile
    {
        public EducationGroupMApper()
        {
            CreateMap<EducationGroups, EducationGroupDto>();

            CreateMap<EducationGroupDto, EducationGroups>();
        }
    }
}
