using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.ViewModels;
using NgSchoolsDataLayer.Enums;
using NgSchoolsDataLayer.Models;
using System.Linq;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class StudentRegisterMapper : Profile
    {
        public StudentRegisterMapper()
        {
            CreateMap<StudentRegister, StudentRegisterDto>()
                .ForMember(dest => dest.NumberOfEntries, opt => opt.MapFrom(src => src.StudentRegisterEntries != null ? src.StudentRegisterEntries.Where(a => a.Status == DatabaseEntityStatusEnum.Active && a.EducationProgram.Status == DatabaseEntityStatusEnum.Active).AsQueryable().Count() : 0))
                .ForMember(dest => dest.MaxEntryNumber, opt => opt.MapFrom(src => src.StudentRegisterEntries != null && src.StudentRegisterEntries.Count > 0 ? src.StudentRegisterEntries.Where(a => a.Status == DatabaseEntityStatusEnum.Active).AsQueryable().Max(sre => sre.StudentRegisterNumber) : 0))
                .ForMember(dest => dest.MinEntryNumber, opt => opt.MapFrom(src => src.StudentRegisterEntries != null && src.StudentRegisterEntries.Count > 0 ? src.StudentRegisterEntries.Where(a => a.Status == DatabaseEntityStatusEnum.Active).AsQueryable().Min(sre => sre.StudentRegisterNumber) : 0));

            CreateMap<StudentRegisterDto, StudentRegister>();

            CreateMap<StudentRegisterEntry, StudentRegisterEntryDto>()
                .ForMember(dest => dest.BookNumber, opt => opt.MapFrom(src => src.StudentRegister.BookNumber))
                .ForMember(dest => dest.StudentGroup, opt => opt.MapFrom(src => src.StudentsInGroups.StudentGroup))
                .ForMember(dest => dest.Student, opt => opt.MapFrom(src => src.StudentsInGroups.Student));

            CreateMap<StudentRegisterEntryDto, StudentRegisterEntry>()
                .ForMember(dest => dest.EducationProgram, opt => opt.Ignore())
                .ForMember(dest => dest.StudentsInGroups, opt => opt.Ignore());

            CreateMap<StudentRegisterEntry, StudentRegisterEntryGridViewModel>()
                .ForMember(dest => dest.StudentRegisterId, opt => opt.MapFrom(src => src.StudentRegister.Id))
                .ForMember(dest => dest.StudentRegisterNumber, opt => opt.MapFrom(src => src.StudentRegister.BookNumber))
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentsInGroups.Student.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.StudentsInGroups.Student.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.StudentsInGroups.Student.LastName))
                .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.StudentsInGroups.StudentGroup.Id))
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.StudentsInGroups.StudentGroup.Name))
                .ForMember(dest => dest.EducationProgramName, opt => opt.MapFrom(src => src.EducationProgram.Name));
        }
    }
}
