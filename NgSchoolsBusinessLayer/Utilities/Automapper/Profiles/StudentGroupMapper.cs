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
                .ForMember(dest => dest.StudentNames, opt => opt.MapFrom(src => src.StudentsInGroups.Select(sig => $"{sig.Student.FirstName} {sig.Student.LastName}")))
                .ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.StudentsInGroups.Select(sig => sig.Student)));

            CreateMap<StudentGroupDto, StudentGroup>()
                .ForMember(dest => dest.ClassLocation, opt => opt.Ignore())
                .ForMember(dest => dest.EducationLeader, opt => opt.Ignore())
                .ForMember(dest => dest.ExamCommission, opt => opt.Ignore())
                .ForMember(dest => dest.Program, opt => opt.Ignore())
                .ForMember(dest => dest.SubjectTeachers, opt => opt.Ignore())
                .ForMember(dest => dest.StudentsInGroups, opt => opt.Ignore())
                .ForMember(dest => dest.StudentGroupClassAttendances, opt => opt.Ignore());

            CreateMap<StudentsInGroups, StudentInGroupDto>()
                .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.StudentGroupId))
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
                .ReverseMap();

            CreateMap<StudentGroupSubjectTeachers, StudentGroupSubjectTeachersDto>().ReverseMap();

            CreateMap<StudentClassAttendance, StudentClassAttendanceDto>().ReverseMap();

            CreateMap<StudentGroupClassAttendance, StudentGroupClassAttendanceDto>();

            CreateMap<StudentGroupClassAttendanceDto, StudentGroupClassAttendance>()
                .ForMember(dest => dest.StudentClassAttendances, opt => opt.Ignore());
        }
    }
}
