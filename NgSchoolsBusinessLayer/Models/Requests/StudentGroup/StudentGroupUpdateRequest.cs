using NgSchoolsBusinessLayer.Models.Dto.StudentGroup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsBusinessLayer.Models.Requests.StudentGroup
{
    public class StudentGroupUpdateRequest
    {
        [Required]
        public int? Id { get; set; }
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

        public int ProgramId { get; set; }

        public int ClassLocationId { get; set; }

        public Guid? EducationLeaderId { get; set; }
        public Guid? DirectorId { get; set; }

        public int? ExamCommissionId { get; set; }
        public int? PracticalExamCommissionId { get; set; }

        public int? CombinedGroupId { get; set; }

        public List<StudentInGroupDto> StudentsInGroup { get; set; }
    }
}
