using NgSchoolsDataLayer.Models.BaseTypes;
using System;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class UserExamCommission : DatabaseEntity
    {
        public UserExamCommission() : base() { }

        [Key]
        public int Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public string CommissionRole { get; set; }
        [Required]
        public int ExamCommissionId { get; set; }
        public virtual ExamCommission ExamCommission { get; set; }
    }
}
