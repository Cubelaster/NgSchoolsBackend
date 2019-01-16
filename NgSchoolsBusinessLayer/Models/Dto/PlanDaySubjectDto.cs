namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class PlanDaySubjectDto
    {
        public int? Id { get; set; }
        public int? PlanDayId { get; set; }
        public int? SubjectId { get; set; }
        public SubjectDto Subject { get; set; }
    }
}
