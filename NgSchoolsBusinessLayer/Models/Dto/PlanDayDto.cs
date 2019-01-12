using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class PlanDayDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PlanDaySubjectDto> PlanDaySubjects { get; set; }
        public List<PlanDayThemeDto> PlanDayThemes { get; set; }
    }
}
