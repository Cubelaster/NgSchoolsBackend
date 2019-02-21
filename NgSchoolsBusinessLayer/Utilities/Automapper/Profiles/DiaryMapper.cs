using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Enums;
using NgSchoolsDataLayer.Models;
using System.Linq;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class DiaryMapper : Profile
    {
        public DiaryMapper()
        {
            CreateMap<Diary, DiaryDto>()
                .ForMember(dest => dest.StudentGroupIds, opt => opt.MapFrom(src => src.StudentGroups.Where(sg => sg.Status == DatabaseEntityStatusEnum.Active).Select(sg => sg.StudentGroupId)))
                .ForMember(dest => dest.StudentGroups, opt => opt.Ignore());

            CreateMap<DiaryDto, Diary>()
                .ForMember(dest => dest.StudentGroups, opt => opt.Ignore());

            CreateMap<DiaryStudentGroup, DiaryStudentGroupDto>().ReverseMap();
        }
    }
}
