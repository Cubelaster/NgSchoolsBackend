using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Enums;
using NgSchoolsDataLayer.Models;
using System.Linq;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class SubjectMapper : Profile
    {
        public SubjectMapper()
        {
            CreateMap<Subject, SubjectDto>()
                .ForMember(dest => dest.Themes, opt => opt.MapFrom(src => src.Themes.Where(a => a.Status == DatabaseEntityStatusEnum.Active)));

            CreateMap<SubjectDto, Subject>()
                .ForMember(dest => dest.Themes, opt => opt.Ignore());

            CreateMap<PlanDaySubjectDto, PlanDaySubject>();

            CreateMap<PlanDaySubject, PlanDaySubjectDto>()
                .ForMember(dest => dest.ThemeIds, opt => opt.MapFrom(src => src.PlanDaySubjectThemes.Where(a => a.Status == DatabaseEntityStatusEnum.Active).Select(pdst => pdst.ThemeId)))
                .ForMember(dest => dest.PlanDaySubjectThemeIds, opt => opt.MapFrom(src => src.PlanDaySubjectThemes.Where(a => a.Status == DatabaseEntityStatusEnum.Active).Select(pdst => pdst.Id)))
                .ForMember(dest => dest.PlanDaySubjectThemes, opt => opt.MapFrom(src => src.PlanDaySubjectThemes.Where(a => a.Status == DatabaseEntityStatusEnum.Active)));
        }
    }
}
