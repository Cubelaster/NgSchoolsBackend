using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class UserExamCommissionMapper : Profile
    {
        public UserExamCommissionMapper()
        {
            CreateMap<UserExamCommission, UserExamCommissionDto>()
                .ForMember(dest => dest.UserDetails, opt => opt.MapFrom(src => src.User.UserDetails));
            CreateMap<UserExamCommissionDto, UserExamCommission>()
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
