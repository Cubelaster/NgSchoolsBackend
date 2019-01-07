using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class EducationProgramMapper : Profile
    {
        public EducationProgramMapper()
        {
            CreateMap<EducationProgram, EducationProgramDto>();

            CreateMap<EducationProgramDto, EducationProgram>();
        }
    }
}
