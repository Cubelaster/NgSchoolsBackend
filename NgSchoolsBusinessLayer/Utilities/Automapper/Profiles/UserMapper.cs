using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.ViewModels;
using NgSchoolsDataLayer.Models;
using System.Linq;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserDetailsId, opt => opt.MapFrom(src => src.UserDetails.Id))
                .ForMember(dest => dest.UserDetails, opt => opt.MapFrom(src => src.UserDetails))
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.Roles.Select(r => new RoleDto { Id = r.RoleId, Name = r.Role.Name })));

            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.UserDetails.Avatar))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.UserDetails.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.UserDetails.LastName))
                .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.UserDetails.Mobile))
                .ForMember(dest => dest.Mobile2, opt => opt.MapFrom(src => src.UserDetails.Mobile2))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.UserDetails.Phone))
                .ForMember(dest => dest.RoleNames, opt => opt.MapFrom(src => string.Join(", ", src.Roles.Select(r => r.Role.Name))))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Role.Id)))
                .ForMember(dest => dest.Signature, opt => opt.MapFrom(src => src.UserDetails.Signature))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.UserDetails.Title));

            CreateMap<UserViewModel, User>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore());

            CreateMap<UserDto, UserViewModel>()
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.UserDetails.Avatar))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.UserDetails.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.UserDetails.LastName))
                .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.UserDetails.Mobile))
                .ForMember(dest => dest.Mobile2, opt => opt.MapFrom(src => src.UserDetails.Mobile2))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.UserDetails.Phone))
                .ForMember(dest => dest.RoleNames, opt => opt.MapFrom(src => string.Join(", ", src.UserRoles.Select(r => r.Name))))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(r => r.Id)))
                .ForMember(dest => dest.Signature, opt => opt.MapFrom(src => src.UserDetails.Signature))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.UserDetails.Title));

            CreateMap<UserDto, TeacherViewModel>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.UserDetails.Address))
                .ForMember(dest => dest.Authorization, opt => opt.MapFrom(src => src.UserDetails.Authorization))
                .ForMember(dest => dest.Bank, opt => opt.MapFrom(src => src.UserDetails.Bank))
                .ForMember(dest => dest.Certificates, opt => opt.MapFrom(src => src.UserDetails.Certificates))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.UserDetails.City))
                .ForMember(dest => dest.EmploymentPlace, opt => opt.MapFrom(src => src.UserDetails.EmploymentPlace));
                //.ForMember(dest => dest.Iban, opt => opt.MapFrom(src => src.iba))
                //.ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(r => r.Id)))
                //.ForMember(dest => dest.Signature, opt => opt.MapFrom(src => src.UserDetails.Signature))
                //.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.UserDetails.Title));

            CreateMap<UserDetails, UserDetailsDto>();

            CreateMap<UserDetails, UserViewModel>();

            CreateMap<UserDetailsDto, UserDetails>();

            CreateMap<UserViewModel, UserDetails>();
        }
    }
}
