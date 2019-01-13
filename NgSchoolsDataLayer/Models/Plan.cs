using NgSchoolsDataLayer.Models.BaseTypes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class Plan : DatabaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public int? EducationPogramId { get; set; }
        public virtual EducationProgram EducationProgram { get; set; }

        public ICollection<PlanDay> PlanDays { get; set; }
    }
}
