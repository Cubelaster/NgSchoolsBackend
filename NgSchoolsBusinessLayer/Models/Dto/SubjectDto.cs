using NgSchoolsBusinessLayer.Utilities.Attributes;
using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class SubjectDto
    {
        public int? Id { get; set; }
        [Searchable]
        public string Name { get; set; }
        public string SubjectCompetence { get; set; }
        public string WorkMethods { get; set; }
        public string MaterialConditions { get; set; }
        public string StaffingConditions { get; set; }
        public string Literature { get; set; }
        public int? CollectiveConsultations { get; set; }
        public int? IndividualConsultations { get; set; }
        public int? InstConsultations { get; set; }
        public int? PracticalClasses { get; set; }
        public int? TheoreticalClasses { get; set; }
        public bool SubjectObligatory { get; set; }

        public int? EducationProgramId { get; set; }
        public List<ThemeDto> Themes { get; set; }
    }
}
