using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class StudentGroupDto
    {
        public int? Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CredentialDate { get; set; }
        public string FirstExamDate { get; set; }
        public string SecondExamDate { get; set; }
        public string Notes { get; set; }
        public int ProgramId { get; set; }
        public int ClassLocationId { get; set; }
        public List<int> StudentIds { get; set; }
        public List<string> Students { get; set; }
        public List<StudentGroupSubjectTeachersDto> SubjectTeachers { get; set; }
    }
}
