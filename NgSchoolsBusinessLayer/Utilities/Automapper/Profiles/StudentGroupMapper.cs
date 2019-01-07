using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class StudentGroupMapper : Profile
    {
        public StudentGroupMapper()
        {
            CreateMap<StudentGroup, StudentGroupDto>();

            CreateMap<StudentGroupDto, StudentGroup>();
        }
    }
}
