using NgSchoolsDataLayer.Models.BaseTypes;
using System;

namespace NgSchoolsDataLayer.Models
{
    public class IssuedPrint : DatabaseEntity
    {
        public int Id { get; set; }

        public int? StudentId { get; set; }
        public Student Student { get; set; }

        public int? EducationProgramId { get; set; }
        public EducationProgram EducationProgram { get; set; }

        public DateTime PrintDate { get; set; }
        public int PrintNumber { get; set; }
    }
}
