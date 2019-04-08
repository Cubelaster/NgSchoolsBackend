using NgSchoolsDataLayer.Models.BaseTypes;
using System.Collections.Generic;

namespace NgSchoolsDataLayer.Models
{
    public class CombinedGroup : DatabaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<StudentGroup> StudentGroups { get; set; }
    }
}
