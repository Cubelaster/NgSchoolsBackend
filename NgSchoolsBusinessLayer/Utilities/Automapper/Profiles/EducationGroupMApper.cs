using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class EducationGroupMapper : Profile
    {
        public EducationGroupMapper()
        {
            CreateMap<EducationGroups, EducationGroupDto>();

            CreateMap<EducationGroupDto, EducationGroups>();
        }
    }
}
