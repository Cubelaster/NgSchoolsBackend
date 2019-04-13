using NgSchoolsBusinessLayer.Utilities.Attributes;
using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class PlanDto
    {
        public int? Id { get; set; }
        [Searchable]
        public string Name { get; set; }
        public int? EducationProgramId { get; set; }

        public List<PlanDayDto> PlanDays { get; set; }
        public List<int> PlanDaysId { get; set; }
    }
}
