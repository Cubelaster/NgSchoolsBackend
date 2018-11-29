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
                .ForMember(dest => dest.RoleNames, opt => opt.MapFrom(src => src.Roles.Select(r => r.Role.Name)))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Role.Id)))
                .ForMember(dest => dest.Signature, opt => opt.MapFrom(src => src.UserDetails.Signature))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.UserDetails.Title));

            CreateMap<UserDto, UserViewModel>()
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.UserDetails.Avatar))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.UserDetails.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.UserDetails.LastName))
                .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.UserDetails.Mobile))
                .ForMember(dest => dest.Mobile2, opt => opt.MapFrom(src => src.UserDetails.Mobile2))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.UserDetails.Phone))
                .ForMember(dest => dest.RoleNames, opt => opt.MapFrom(src => src.UserRoles.Select(r => r.Name)))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(r => r.Id)))
                .ForMember(dest => dest.Signature, opt => opt.MapFrom(src => src.UserDetails.Signature))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.UserDetails.Title));

            CreateMap<UserDetails, UserDetailsDto>();

            CreateMap<UserDetailsDto, UserDetails>();

            //CreateMap<UserDto, User>()
            //    .ForMember(dest => dest.UserDetails, opt => opt.MapFrom(src => src.UserDetailsId.HasValue ?
            //    new UserDetails
            //    {
            //        Id = src.UserDetailsId.Value,
            //        Avatar = src.Avatar,
            //        FirstName = src.FirstName,
            //        LastName = src.LastName,
            //        Mobile = src.Mobile,
            //        Mobile2 = src.Mobile2,
            //        Phone = src.Phone,
            //        Signature = src.Signature,
            //        Title = src.Title,
            //        UserId = src.Id
            //    } : null))
            //    .ForMember(dest => dest.SecurityStamp, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            //    .ForMember(dest => dest.Roles, opt => opt.Ignore());
        }
    }
}
