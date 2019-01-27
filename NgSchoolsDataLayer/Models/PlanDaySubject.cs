using NgSchoolsDataLayer.Models.BaseTypes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NgSchoolsDataLayer.Models
{
    public class PlanDaySubject : DatabaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PlanDayId { get; set; }
        public virtual PlanDay PlanDay { get; set; }

        [Required]
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public virtual ICollection<PlanDaySubjectTheme> PlanDaySubjectThemes { get; set; }
    }
}
