using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class EducationProgramMapper : Profile
    {
        public EducationProgramMapper()
        {
            CreateMap<EducationProgram, EducationProgramDto>()
                .ForMember(dest => dest.EducationGroup, opt => opt.MapFrom(src => src.EducationGroup))
                .ForMember(dest => dest.ClassTypeIds, opt => opt.MapFrom(src => src.EducationProgramClassTypes.Select(epct => epct.ClassTypeId)))
                .ForMember(dest => dest.ClassTypes, opt => opt.MapFrom(src => src.EducationProgramClassTypes != null ? src.EducationProgramClassTypes.Select(epct => epct.ClassType).ToList() : new List<ClassType>()));

            CreateMap<EducationProgramDto, EducationProgram>()
                .ForMember(dest => dest.EducationGroup, opt => opt.Ignore());

            CreateMap<EducationProgramClassType, EducationProgramClassTypeDto>().ReverseMap();
        }
    }
}
