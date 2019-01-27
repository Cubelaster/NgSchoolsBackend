namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class PlanDaySubjectThemeDto
    {
        public int? Id { get; set; }

        public int? PlanDaySubjectId { get; set; }

        public int? ThemeId { get; set; }
        public ThemeDto Theme { get; set; }

        public double? HoursNumber { get; set; }
    }
}
