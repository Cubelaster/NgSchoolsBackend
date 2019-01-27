using NgSchoolsDataLayer.Models.BaseTypes;
using System.Collections.Generic;

namespace NgSchoolsDataLayer.Models
{
    public class PlanDay : DatabaseEntity
    {
        public int Id { get; set; }
        public int PlanId { get; set; }
        public Plan Plan { get; set; }

        public virtual ICollection<PlanDaySubject> Subjects { get; set; }
    }
}
