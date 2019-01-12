using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class PlanMapper : Profile
    {
        public PlanMapper()
        {
            CreateMap<Plan, PlanDto>().ReverseMap();

            CreateMap<PlanDay, PlanDayDto>().ReverseMap();
        }
    }
}
