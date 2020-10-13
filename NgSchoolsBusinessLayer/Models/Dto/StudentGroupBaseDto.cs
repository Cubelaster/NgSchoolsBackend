using Core.Utilities.Attributes;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class StudentGroupBaseDto
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
    }
}
