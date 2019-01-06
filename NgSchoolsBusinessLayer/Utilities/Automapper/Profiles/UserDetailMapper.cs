﻿using AutoMapper;
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

            CreateMap<UserDetailsDto, UserDetails>();

            CreateMap<UserViewModel, UserDetailsDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

            CreateMap<TeacherViewModel, UserDetailsDto>();
        }
    }
}