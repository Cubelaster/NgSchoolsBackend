﻿using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Dto.StudentGroup;
using NgSchoolsBusinessLayer.Models.Requests.StudentGroup;
using NgSchoolsBusinessLayer.Models.ViewModels.StudentGroup;
using NgSchoolsDataLayer.Enums;
using NgSchoolsDataLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class StudentGroupMapper : Profile
    {
        public StudentGroupMapper()
        {
            CreateMap<StudentGroup, StudentGroupDto>()
                .ForMember(dest => dest.EducationProgram, opt => opt.MapFrom(src => src.Program))
                .ForMember(dest => dest.SubjectTeachers, opt => opt.MapFrom(src => src.SubjectTeachers.Where(st => st.Status == DatabaseEntityStatusEnum.Active)))
                .ForMember(dest => dest.StudentGroupClassAttendances, opt => opt.MapFrom(src => src.StudentGroupClassAttendances.Where(sgca => sgca.Status == DatabaseEntityStatusEnum.Active)))
                .ForMember(dest => dest.StudentIds, opt => opt.MapFrom(src => src.StudentsInGroups.Where(sig => sig.Status == DatabaseEntityStatusEnum.Active).Select(sig => sig.StudentId)))
                .ForMember(dest => dest.StudentNames, opt => opt.MapFrom(src => src.StudentsInGroups.Where(sig => sig.Status == DatabaseEntityStatusEnum.Active).Select(sig => $"{sig.Student.FirstName} {sig.Student.LastName}")))
                .ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.StudentsInGroups.Where(sig => sig.Status == DatabaseEntityStatusEnum.Active).Select(sig => sig.Student)))
                .ForMember(dest => dest.StudentsInGroup, opt => opt.MapFrom(src => src.StudentsInGroups.Where(sig => sig.Status == DatabaseEntityStatusEnum.Active)));

            CreateMap<StudentGroup, StudentGroupBaseDto>()
                .ForMember(dest => dest.EducationProgram, opt => opt.MapFrom(src => src.Program));

            CreateMap<StudentGroupDto, StudentGroup>()
                .ForMember(dest => dest.ClassLocation, opt => opt.Ignore())
                .ForMember(dest => dest.EducationLeader, opt => opt.Ignore())
                .ForMember(dest => dest.ExamCommission, opt => opt.Ignore())
                .ForMember(dest => dest.PracticalExamCommission, opt => opt.Ignore())
                .ForMember(dest => dest.Program, opt => opt.Ignore())
                .ForMember(dest => dest.SubjectTeachers, opt => opt.Ignore())
                .ForMember(dest => dest.StudentsInGroups, opt => opt.Ignore())
                .ForMember(dest => dest.Director, opt => opt.Ignore())
                .ForMember(dest => dest.StudentGroupClassAttendances, opt => opt.Ignore());

            CreateMap<StudentsInGroups, StudentInGroupBaseDto>()
                .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.StudentGroupId))
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
                .ForMember(dest => dest.StudentRegisterNumber, opt => opt.MapFrom(src => src.StudentRegisterEntry != null ? src.StudentRegisterEntry.StudentRegisterNumber : (int?)null));

            CreateMap<StudentsInGroups, StudentInGroupDto>()
                .IncludeBase<StudentsInGroups, StudentInGroupBaseDto>();

            CreateMap<StudentInGroupDto, StudentsInGroups>()
                .ForMember(dest => dest.StudentGroupId, opt => opt.MapFrom(src => src.GroupId))
                .ForMember(dest => dest.Employer, opt => opt.Ignore())
                .ForMember(dest => dest.StudentExamEvidences, opt => opt.Ignore());

            CreateMap<StudentExamEvidence, StudentExamEvidenceDto>().ReverseMap();

            CreateMap<StudentGroupSubjectTeachers, StudentGroupSubjectTeachersDto>().ReverseMap();

            CreateMap<StudentClassAttendance, StudentClassAttendanceDto>().ReverseMap();

            CreateMap<StudentGroupClassAttendance, StudentGroupClassAttendanceDto>();

            CreateMap<StudentGroupClassAttendanceDto, StudentGroupClassAttendance>();
                //.ForMember(dest => dest.StudentClassAttendances, opt => opt.Ignore());

            CreateMap<CombinedGroup, CombinedGroupDto>()
                .ForMember(dest => dest.StudentGroups, opt => opt.MapFrom(src => src.StudentGroups != null ? src.StudentGroups.Where(sg => sg.Status == DatabaseEntityStatusEnum.Active).ToList() : new List<StudentGroup>()))
                .ForMember(dest => dest.StudentGroupIds, opt => opt.MapFrom(src => src.StudentGroups != null ? src.StudentGroups.Where(sg => sg.Status == DatabaseEntityStatusEnum.Active).Select(sg => sg.Id).ToList() : new List<int>()));

            CreateMap<CombinedGroupDto, CombinedGroup>()
                .ForMember(dest => dest.StudentGroups, opt => opt.Ignore());

            #region View Models

            CreateMap<StudentGroup, StudentGroupGridViewModel>();

            CreateMap<StudentGroup, StudentGroupDetailsViewModel>()
                .ForMember(dest => dest.EducationLeader, opt => opt.Ignore())
                .ForMember(dest => dest.Director, opt => opt.Ignore())
                .ForMember(dest => dest.StudentsInGroup, opt => opt.MapFrom(src => src.StudentsInGroups.Where(sig => sig.Status == DatabaseEntityStatusEnum.Active)));

            #endregion View Models

            #region Requests

            CreateMap<StudentGroupUpdateRequest, StudentGroup>();

            #endregion Requests
        }
    }
}
