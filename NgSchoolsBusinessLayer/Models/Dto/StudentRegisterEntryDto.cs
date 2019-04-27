﻿using NgSchoolsBusinessLayer.Enums;
using NgSchoolsBusinessLayer.Utilities.Attributes;
using System;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    [Cached(CacheKeysEnum.StudentRegisterEntry)]
    public class StudentRegisterEntryDto
    {
        public int? Id { get; set; }
        public int? StudentRegisterNumber { get; set; }
        public string Notes { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime? ExamDate { get; set; }
        public int? ExamDateNumber { get; set; }

        public int? StudentRegisterId { get; set; }

        public int? EducationProgramId { get; set; }
        public EducationProgramDto EducationProgram { get; set; }

        public int? StudentsInGroupsId { get; set; }
        public StudentInGroupDto StudentsInGroups { get; set; }

        public StudentDto Student { get; set; }
        public StudentGroupDto StudentGroup { get; set; }
    }
}
