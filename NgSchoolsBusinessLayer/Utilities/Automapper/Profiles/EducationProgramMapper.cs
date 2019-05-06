using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Enums;
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
                .ForMember(dest => dest.Subjects, opt => opt.MapFrom(src => src.Subjects.Where(s => s.Status == DatabaseEntityStatusEnum.Active)))
                .ForMember(dest => dest.ClassTypeIds, opt => opt.MapFrom(src => src.EducationProgramClassTypes.Where(epct => epct.Status == DatabaseEntityStatusEnum.Active).Select(epct => epct.ClassTypeId)))
                .ForMember(dest => dest.ClassTypes, opt => opt.MapFrom(src => src.EducationProgramClassTypes != null ? src.EducationProgramClassTypes.Where(epct => epct.Status == DatabaseEntityStatusEnum.Active).Select(epct => epct.ClassType).ToList() : new List<ClassType>()))
                .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.Files != null ? src.Files.Where(f => f.Status == DatabaseEntityStatusEnum.Active).Select(f => new FileDto { Id = f.File.Id, FileName = f.File.FileName }).ToList() : new List<FileDto>()));

            CreateMap<EducationProgramDto, EducationProgram>()
                .ForMember(dest => dest.EducationGroup, opt => opt.Ignore())
                .ForMember(dest => dest.Files, opt => opt.Ignore());

            CreateMap<EducationProgramClassType, EducationProgramClassTypeDto>().ReverseMap();

            CreateMap<EducationProgramFile, EducationProgramFileDto>().ReverseMap();
        }
    }
}
