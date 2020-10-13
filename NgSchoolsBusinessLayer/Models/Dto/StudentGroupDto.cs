using NgSchoolsBusinessLayer.Models.Dto.StudentGroup;
using Core.Utilities.Attributes;
using System;
using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class StudentGroupDto
    {
        public int? Id { get; set; }
        [Searchable]
        public string Name { get; set; }
        public string SchoolYear { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CredentialDate { get; set; }
        public string FirstExamDate { get; set; }
        public string SecondExamDate { get; set; }
        public string PracticalExamFirstDate { get; set; }
        public string PracticalExamSecondDate { get; set; }
        public string Notes { get; set; }
        public string EnrolmentDate { get; set; }
        public string EducationGroupMark { get; set; }

        public int? ProgramId { get; set; }
        public EducationProgramDto EducationProgram { get; set; }

        public int? ClassLocationId { get; set; }
        public ClassLocationsDto ClassLocation { get; set; }

        public List<int> StudentIds { get; set; }
        public List<string> StudentNames { get; set; }
        public List<StudentDto> Students { get; set; }
        public List<StudentInGroupDto> StudentsInGroup { get; set; }
        public List<StudentGroupSubjectTeachersDto> SubjectTeachers { get; set; }

        public Guid? EducationLeaderId { get; set; }
        public UserDto EducationLeader { get; set; }

        public int? ExamCommissionId { get; set; }
        public ExamCommissionDto ExamCommission { get; set; }

        public int? PracticalExamCommissionId { get; set; }
        public virtual ExamCommissionDto PracticalExamCommission { get; set; }

        public Guid? DirectorId { get; set; }
        public UserDto Director { get; set; }

        public int? CombinedGroupId { get; set; }

        public List<StudentGroupClassAttendanceDto> StudentGroupClassAttendances { get; set; }
    }
}
