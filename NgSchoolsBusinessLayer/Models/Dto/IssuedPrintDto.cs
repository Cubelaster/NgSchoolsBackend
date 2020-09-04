using System;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class IssuedPrintDto
    {
        public int? Id { get; set; }

        public int? StudentId { get; set; }
        public StudentBaseDto Student { get; set; }

        public int? EducationProgramId { get; set; }
        public EducationProgramBaseDto EducationProgram { get; set; }

        public DateTime PrintDate { get; set; }

        public int PrintNumber { get; set; }
        public int PrintDuplicateNumber { get; set; }
    }
}
