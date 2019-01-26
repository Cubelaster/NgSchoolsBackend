namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class ThemeDto
    {
        public int? Id { get; set; }
        public int? SubjectId { get; set; }
        public string Name { get; set; }
        public int? HoursNumber { get; set; }
        public string Content { get; set; }
        public string LearningOutcomes { get; set; }
    }
}
