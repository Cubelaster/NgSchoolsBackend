using NgSchoolsDataLayer.Models.BaseTypes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class Subject : DatabaseEntity
    {
        [Key]
        public int Id { get; set; }
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
        public bool IsPracticalType { get; set; }

        public int EducationProgramId { get; set; }
        public EducationProgram EducationProgram { get; set; }

        public virtual ICollection<Theme> Themes { get; set; }
        public virtual ICollection<PlanDaySubject> PlanDaySubjects { get; set; }
    }
}
