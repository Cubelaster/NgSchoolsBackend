using NgSchoolsDataLayer.Models.BaseTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NgSchoolsDataLayer.Models
{
    public class PlanDaySubject : DatabaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
        [Required]
        public int PlanDayId { get; set; }
        public virtual PlanDay PlanDay { get; set; }
    }
}
