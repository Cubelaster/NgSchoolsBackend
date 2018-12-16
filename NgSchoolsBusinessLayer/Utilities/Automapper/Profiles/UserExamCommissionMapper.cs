using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class UserExamCommissionMapper : Profile
    {
        public UserExamCommissionMapper()
        {
            CreateMap<UserExamCommission, UserExamCommissionDto>();
            CreateMap<UserExamCommissionDto, UserExamCommission>();
        }
    }
}
