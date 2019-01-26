using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class SubjectMapper : Profile
    {
        public SubjectMapper()
        {
            CreateMap<Subject, SubjectDto>().ReverseMap();

            CreateMap<PlanDaySubject, PlanDaySubjectDto>().ReverseMap();

            //CreateMap<SubjectTheme, SubjectThemeDto>().ReverseMap();
        }
    }
}
