namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class StudentExamEvidenceDto
    {
        public int? Id { get; set; }
        public string ExamDate { get; set; }
        public string ExamEvidence { get; set; }
        public int? StudentsInGroupsId { get; set; }
    }
}
