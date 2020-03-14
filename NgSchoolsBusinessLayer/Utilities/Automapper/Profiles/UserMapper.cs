using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.ViewModels;
using NgSchoolsBusinessLayer.Models.ViewModels.User;
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

            CreateMap<UserDto, UserBaseViewModel>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.UserDetails.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.UserDetails.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.UserDetails.LastName));

            CreateMap<UserDto, TeacherViewModel>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.UserDetails.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.UserDetails.LastName))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Oib, opt => opt.MapFrom(src => src.UserDetails.Oib))
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.UserDetails.CityId))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.UserDetails.City))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.UserDetails.Country))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.UserDetails.CountryId))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.UserDetails.Region))
                .ForMember(dest => dest.RegionId, opt => opt.MapFrom(src => src.UserDetails.RegionId))
                .ForMember(dest => dest.Municipality, opt => opt.MapFrom(src => src.UserDetails.Municipality))
                .ForMember(dest => dest.MunicipalityId, opt => opt.MapFrom(src => src.UserDetails.MunicipalityId))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.UserDetails.Notes))
                .ForMember(dest => dest.PpEducation, opt => opt.MapFrom(src => src.UserDetails.PpEducation))
                .ForMember(dest => dest.Profession, opt => opt.MapFrom(src => src.UserDetails.Profession))
                .ForMember(dest => dest.Qualifications, opt => opt.MapFrom(src => src.UserDetails.Qualifications))
                .ForMember(dest => dest.Iban, opt => opt.MapFrom(src => src.UserDetails.Iban))
                .ForMember(dest => dest.Bank, opt => opt.MapFrom(src => src.UserDetails.Bank))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.UserDetails.Address))
                .ForMember(dest => dest.Authorization, opt => opt.MapFrom(src => src.UserDetails.Authorization))
                .ForMember(dest => dest.Bank, opt => opt.MapFrom(src => src.UserDetails.Bank))
                .ForMember(dest => dest.Certificates, opt => opt.MapFrom(src => src.UserDetails.Certificates))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.UserDetails.City))
                .ForMember(dest => dest.EmploymentPlace, opt => opt.MapFrom(src => src.UserDetails.EmploymentPlace))
                .ForMember(dest => dest.RoleNames, opt => opt.MapFrom(src => string.Join(", ", src.UserRoles.Select(r => r.Name))))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(r => r.Id)))
                .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.UserDetails.TeacherFiles));

            CreateMap<Role, RoleDto>();

            CreateMap<TeacherViewModel, UserViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId));
        }
    }
}
