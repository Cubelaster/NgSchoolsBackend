using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class EducationLevelMapper : Profile
    {
        public EducationLevelMapper()
        {
            CreateMap<EducationLevel, EducationLevelDto>().ReverseMap();
        }
    }
}
