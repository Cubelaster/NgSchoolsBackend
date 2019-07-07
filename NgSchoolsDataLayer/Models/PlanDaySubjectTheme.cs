using NgSchoolsDataLayer.Models.BaseTypes;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class PlanDaySubjectTheme : DatabaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ThemeId { get; set; }
        public virtual Theme Theme { get; set; }

        [Required]
        public int PlanDaySubjectId { get; set; }
        public virtual PlanDaySubject PlanDaySubject { get; set; }

        public double HoursNumber { get; set; }
        public string ClassTypes { get; set; }
        public string PerfomingType { get; set; }
    }
}
