using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class PlanDaySubjectDto
    {
        public int? Id { get; set; }
        public int? PlanDayId { get; set; }
        public int? SubjectId { get; set; }
        public SubjectDto Subject { get; set; }

        public List<int> PlanDaySubjectThemeIds { get; set; }
        public List<int> ThemeIds { get; set; }
        public List<PlanDaySubjectThemeDto> PlanDaySubjectThemes { get; set; }
    }
}
