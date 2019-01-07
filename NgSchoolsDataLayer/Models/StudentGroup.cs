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
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CredentialDate { get; set; }
        public DateTime FirstExamDate { get; set; }
        public DateTime SecondExamDate { get; set; }
        [Required]
        public int ProgramId { get; set; }
        public EducationProgram Program { get; set; }

        public ICollection<StudentsInGroups> StudentsInGroups { get; set; }
    }
}
