using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Enums;
using NgSchoolsDataLayer.Models;
using System.Linq;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class PlanMapper : Profile
    {
        public PlanMapper()
        {
            CreateMap<Plan, PlanDto>()
                .ForMember(dest => dest.PlanDays, opt => opt.MapFrom(src => src.PlanDays.Where(a => a.Status == DatabaseEntityStatusEnum.Active)))
                .ForMember(dest => dest.PlanDaysId, opt => opt.MapFrom(src => src.PlanDays.Where(a => a.Status == DatabaseEntityStatusEnum.Active).Select(pd => pd.Id).ToList()));

            CreateMap<PlanDto, Plan>();

            CreateMap<PlanDay, PlanDayDto>()
                .ForMember(dest => dest.PlanDaySubjectIds, opt => opt.MapFrom(src => src.Subjects.Where(a => a.Status == DatabaseEntityStatusEnum.Active).Select(pds => pds.Id)))
                .ForMember(dest => dest.PlanDaySubjects, opt => opt.MapFrom(src => src.Subjects.Where(a => a.Status == DatabaseEntityStatusEnum.Active)));

            CreateMap<PlanDayDto, PlanDay>();
        }
    }
}
