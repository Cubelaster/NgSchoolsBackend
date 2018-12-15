using NgSchoolsDataLayer.Models.BaseTypes;
using System;

namespace NgSchoolsDataLayer.Models
{
    public class UserExamCommission : DatabaseEntity
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public int ExamCommissionId { get; set; }
        public virtual ExamCommission ExamCommission { get; set; }
    }
}
