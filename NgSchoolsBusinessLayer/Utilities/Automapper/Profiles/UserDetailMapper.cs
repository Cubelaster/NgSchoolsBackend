using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.ViewModels;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class UserDetailMapper : Profile
    {
        public UserDetailMapper()
        {
            CreateMap<UserDetails, UserDetailsDto>();

            CreateMap<UserDetailsDto, UserDetails>()
                .ForMember(dest => dest.Avatar, opt => opt.Ignore())
                .ForMember(dest => dest.Signature, opt => opt.Ignore())
                .ForMember(dest => dest.City, opt => opt.Ignore());

            CreateMap<UserViewModel, UserDetailsDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

            CreateMap<TeacherViewModel, UserDetailsDto>()
                .ForMember(dest => dest.City, opt => opt.Ignore());
        }
    }
}
