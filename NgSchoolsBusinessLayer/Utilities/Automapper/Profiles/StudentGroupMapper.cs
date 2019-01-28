using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;
using System.Linq;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class StudentGroupMapper : Profile
    {
        public StudentGroupMapper()
        {
            CreateMap<StudentGroup, StudentGroupDto>()
                .ForMember(dest => dest.StudentIds, opt => opt.MapFrom(src => src.StudentsInGroups.Select(sig => sig.StudentId)))
                .ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.StudentsInGroups.Select(sig => $"{sig.Student.FirstName} {sig.Student.LastName}")));

            CreateMap<StudentGroupDto, StudentGroup>();

            CreateMap<StudentsInGroups, StudentInGroupDto>()
                .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.StudentGroupId))
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
                .ReverseMap();

            CreateMap<StudentGroupSubjectTeachers, StudentGroupSubjectTeachersDto>().ReverseMap();
        }
    }
}
