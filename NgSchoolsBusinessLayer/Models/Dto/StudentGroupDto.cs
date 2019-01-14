using System;
using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class StudentGroupDto
    {
        public int? Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CredentialDate { get; set; }
        public DateTime? FirstExamDate { get; set; }
        public DateTime? SecondExamDate { get; set; }
        public string Notes { get; set; }
        public int ProgramId { get; set; }
        public int ClassLocationId { get; set; }
        public List<int> StudentIds { get; set; }
        public List<string> Students { get; set; }
    }
}
