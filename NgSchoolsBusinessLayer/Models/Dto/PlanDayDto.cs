using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class PlanDayDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? PlanId { get; set; }

        public List<int> PlanDaySubjectIds { get; set; }
        public List<int> SubjectIds { get; set; }

        public List<PlanDaySubjectDto> PlanDaySubjects { get; set; }
    }
}
