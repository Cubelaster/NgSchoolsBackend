using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class DiaryMapper : Profile
    {
        public DiaryMapper()
        {
            CreateMap<Diary, DiaryDto>().ReverseMap();

            CreateMap<DiaryStudentGroup, DiaryStudentGroupDto>().ReverseMap();
        }
    }
}
