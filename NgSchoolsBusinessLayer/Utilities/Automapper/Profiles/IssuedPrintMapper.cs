using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class IssuedPrintMapper : Profile
    {
        public IssuedPrintMapper()
        {
            CreateMap<IssuedPrint, IssuedPrintDto>()
                .ForMember(dest => dest.PrintDuplicateNumber, opt => opt.MapFrom(src => src.PrintNumber > 0 ? src.PrintNumber - 1 : src.PrintNumber));

            CreateMap<IssuedPrintDto, IssuedPrint>()
                .ForMember(dest => dest.EducationProgram, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore());
        }
    }
}
