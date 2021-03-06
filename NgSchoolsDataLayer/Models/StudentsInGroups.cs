﻿using NgSchoolsDataLayer.Models.BaseTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class StudentsInGroups : DatabaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        [Required]
        public int StudentGroupId { get; set; }
        public virtual StudentGroup StudentGroup { get; set; }
        public bool CompletedPractice { get; set; }
        public int? EmployerId { get; set; }
        public BusinessPartner Employer { get; set; }
        public virtual StudentRegisterEntry StudentRegisterEntry { get; set; }

        public DateTime? PracticalStartDate { get; set; }
        public DateTime? PracticalEndDate { get; set; }

        public virtual ICollection<StudentExamEvidence> StudentExamEvidences { get; set; }
    }
}
