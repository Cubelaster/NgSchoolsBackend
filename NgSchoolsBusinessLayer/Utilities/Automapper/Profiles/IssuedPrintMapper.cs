using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class IssuedPrintMapper : Profile
    {
        public IssuedPrintMapper()
        {
            CreateMap<IssuedPrint, IssuedPrintDto>();

            CreateMap<IssuedPrintDto, IssuedPrint>()
                .ForMember(dest => dest.EducationProgram, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore());
        }
    }
}
