using NgSchoolsBusinessLayer.Enums;
using NgSchoolsBusinessLayer.Models.Dto.StudentGroup;
using NgSchoolsBusinessLayer.Utilities.Attributes;
using System;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    [Cached(CacheKeysEnum.StudentRegisterEntry)]
    public class StudentRegisterEntryDto
    {
        public int? Id { get; set; }
        [Searchable]
        public int? StudentRegisterNumber { get; set; }
        public string Notes { get; set; }

        [Searchable]
        public DateTime EntryDate { get; set; }
        public DateTime? ExamDate { get; set; }
        public int? ExamDateNumber { get; set; }

        public int? StudentRegisterId { get; set; }
        [Searchable]
        public string BookNumber { get; set; }

        public int? EducationProgramId { get; set; }
        [Searchable]
        public EducationProgramDto EducationProgram { get; set; }

        public int? StudentsInGroupsId { get; set; }
        public StudentInGroupDto StudentsInGroups { get; set; }

        [Searchable]
        public StudentDto Student { get; set; }

        [Searchable]
        public StudentGroupDto StudentGroup { get; set; }
    }
}
