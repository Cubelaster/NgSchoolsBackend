using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;
using System.Linq;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class PlanMapper : Profile
    {
        public PlanMapper()
        {
            CreateMap<Plan, PlanDto>()
                .ForMember(dest => dest.PlanDaysId, opt => opt.MapFrom(src => src.PlanDays.Select(pd => pd.Id).ToList()));

            CreateMap<PlanDto, Plan>();

            CreateMap<PlanDay, PlanDayDto>().ReverseMap();
        }
    }
}
