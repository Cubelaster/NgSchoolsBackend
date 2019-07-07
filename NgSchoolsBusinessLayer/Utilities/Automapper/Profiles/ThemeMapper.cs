using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class ThemeMapper : Profile
    {
        public ThemeMapper()
        {
            CreateMap<Theme, ThemeDto>()
                .ForMember(dest => dest.Subject, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<PlanDaySubjectThemeDto, PlanDaySubjectTheme>().ReverseMap();
        }
    }
}
