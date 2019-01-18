using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class ThemeMapper : Profile
    {
        public ThemeMapper()
        {
            CreateMap<Theme, ThemeDto>().ReverseMap();

            CreateMap<PlanDayTheme, PlanDayThemeDto>()
                .ForMember(dest => dest.Theme, opt => opt.MapFrom(src => src.Theme));

            CreateMap<PlanDayThemeDto, PlanDayTheme>();
        }
    }
}
