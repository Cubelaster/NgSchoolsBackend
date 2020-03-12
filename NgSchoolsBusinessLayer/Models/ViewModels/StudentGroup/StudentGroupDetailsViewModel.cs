﻿using NgSchoolsBusinessLayer.Utilities.Attributes;
using System;

namespace NgSchoolsBusinessLayer.Models.ViewModels.StudentGroup
{
    public class StudentGroupDetailsViewModel
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
        public int? ClassLocationId { get; set; }

        public Guid? EducationLeaderId { get; set; }
        public int? ExamCommissionId { get; set; }
        public int? PracticalExamCommissionId { get; set; }

        public Guid? DirectorId { get; set; }

        public int? CombinedGroupId { get; set; }
    }
}