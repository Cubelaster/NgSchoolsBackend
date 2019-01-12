using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class PlanDayDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SubjectDto> Subjects { get; set; }
        public List<ThemeDto> Themes { get; set; }
    }
}
