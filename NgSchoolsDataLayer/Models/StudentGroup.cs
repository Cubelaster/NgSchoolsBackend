using NgSchoolsDataLayer.Models.BaseTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class StudentGroup : DatabaseEntity 
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string SchoolYear { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CredentialDate { get; set; }
        public DateTime FirstExamDate { get; set; }
        public DateTime SecondExamDate { get; set; }
        public string Notes { get; set; }
        public DateTime? EnrolmentDate { get; set; }

        [Required]
        public int ProgramId { get; set; }
        public virtual EducationProgram Program { get; set; }
        public int? ClassLocationId { get; set; }
        public virtual ClassLocations ClassLocation { get; set; }

        public ICollection<StudentsInGroups> StudentsInGroups { get; set; }
        public ICollection<StudentGroupSubjectTeachers> SubjectTeachers { get; set; }

        public Guid? EducationLeaderId { get; set; }
        public virtual User EducationLeader { get; set; }

        public int? ExamCommissionId { get; set; }
        public virtual ExamCommission ExamCommission { get; set; }

        public virtual ICollection<StudentGroupClassAttendance> StudentGroupClassAttendances { get; set; }
    }
}
