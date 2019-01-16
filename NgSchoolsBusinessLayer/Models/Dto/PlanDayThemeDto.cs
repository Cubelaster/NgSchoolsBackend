namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class PlanDayThemeDto
    {
        public int? Id { get; set; }
        public int? PlanDayId { get; set; }
        public int? ThemeId { get; set; }
        public ThemeDto Theme { get; set; }
        public double? HoursNumber { get; set; }
    }
}
