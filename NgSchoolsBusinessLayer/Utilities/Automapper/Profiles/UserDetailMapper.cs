using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.ViewModels;
using NgSchoolsDataLayer.Enums;
using NgSchoolsDataLayer.Models;
using System.Linq;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class UserDetailMapper : Profile
    {
        public UserDetailMapper()
        {
            CreateMap<UserDetails, UserDetailsDto>()
                .ForMember(dest => dest.TeacherFiles, opt => opt.MapFrom(src => src.TeacherFiles.Where(a => a.Status == DatabaseEntityStatusEnum.Active).Select(tf => tf.File)));

            CreateMap<UserDetailsDto, UserDetails>()
                .ForMember(dest => dest.Avatar, opt => opt.Ignore())
                .ForMember(dest => dest.Signature, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.Municipality, opt => opt.Ignore())
                .ForMember(dest => dest.Region, opt => opt.Ignore())
                .ForMember(dest => dest.City, opt => opt.Ignore())
                .ForMember(dest => dest.TeacherFiles, opt => opt.Ignore());

            CreateMap<UserViewModel, UserDetailsDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

            CreateMap<TeacherViewModel, UserDetailsDto>()
                .ForMember(dest => dest.City, opt => opt.Ignore())
                .ForMember(dest => dest.Municipality, opt => opt.Ignore())
                .ForMember(dest => dest.Region, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.TeacherFiles, opt => opt.MapFrom(src => src.Files));

            CreateMap<UserDetailsDto, TeacherViewModel>()
                .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.TeacherFiles));

            CreateMap<TeacherFile, TeacherFileDto>().ReverseMap();
        }
    }
}
