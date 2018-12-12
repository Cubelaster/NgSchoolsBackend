using NgSchoolsDataLayer.Models.BaseTypes;

namespace NgSchoolsDataLayer.Models
{
    public class UserExamCommission : DatabaseEntity
    {
        public int Id { get; set; }
        public virtual User User { get; set; }
        public virtual ExamCommission ExamCommission { get; set; }
    }
}
