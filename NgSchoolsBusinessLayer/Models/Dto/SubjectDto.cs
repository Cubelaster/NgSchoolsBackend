using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class SubjectDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string SubjectCompetence { get; set; }
        public string WorkMethods { get; set; }
        public string MaterialConditions { get; set; }
        public string StaffingConditions { get; set; }
        public string Literature { get; set; }

        public List<ThemeDto> Themes { get; set; }
    }
}
