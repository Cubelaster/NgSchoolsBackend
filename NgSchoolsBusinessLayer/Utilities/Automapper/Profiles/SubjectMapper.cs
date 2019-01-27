using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;
using System.Linq;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class SubjectMapper : Profile
    {
        public SubjectMapper()
        {
            CreateMap<Subject, SubjectDto>().ReverseMap();

            CreateMap<PlanDaySubjectDto, PlanDaySubject>();

            CreateMap<PlanDaySubject, PlanDaySubjectDto>()
                .ForMember(dest => dest.PlanDaySubjectThemeIds, opt => opt.MapFrom(src => src.PlanDaySubjectThemes.Select(pdst => pdst.Id)))
                .ForMember(dest => dest.PlanDaySubjectThemes, opt => opt.MapFrom(src => src.PlanDaySubjectThemes));
        }
    }
}
