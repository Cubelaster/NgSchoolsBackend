using NgSchoolsDataLayer.Models.BaseTypes;
using System.Collections.Generic;

namespace NgSchoolsDataLayer.Models
{
    public class ExamCommission : DatabaseEntity
    {
        public ExamCommission() : base() { }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserExamCommission> UserExamCommissions { get; set; }
    }
}
