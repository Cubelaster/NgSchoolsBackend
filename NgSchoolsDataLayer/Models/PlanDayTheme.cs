using NgSchoolsDataLayer.Models.BaseTypes;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class PlanDayTheme : DatabaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ThemeId { get; set; }
        public virtual Theme Theme { get; set; }
        [Required]
        public int PlanDayId { get; set; }
        public virtual PlanDay PlanDay { get; set; }
        public double HoursNumber { get; set; }
    }
}
