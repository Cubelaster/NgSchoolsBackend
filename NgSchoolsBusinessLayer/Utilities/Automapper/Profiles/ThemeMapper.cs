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

            CreateMap<PlanDaySubjectThemeDto, PlanDaySubjectTheme>().ReverseMap();
        }
    }
}
