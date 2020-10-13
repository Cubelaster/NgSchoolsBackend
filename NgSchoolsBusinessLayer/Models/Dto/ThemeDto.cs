using Core.Utilities.Attributes;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class ThemeDto
    {
        public int? Id { get; set; }
        public int? SubjectId { get; set; }
        [Searchable]
        public string Name { get; set; }
        public string Content { get; set; }
        public string LearningOutcomes { get; set; }
        public int? HoursNumber { get; set; }
        public string WorkShopClasses { get; set; }
        public SubjectDto Subject { get; set; }
    }
}
