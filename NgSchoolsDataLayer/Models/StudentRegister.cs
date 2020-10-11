using System.Collections.Generic;
using NgSchoolsDataLayer.Models.BaseTypes;

namespace NgSchoolsDataLayer.Models
{
    public class StudentRegister : DatabaseEntity
    {
        public int Id { get; set; }
        public int BookNumber { get; set; }
        public int? BookYear { get; set; }
        public bool Full { get; set; }

        public virtual ICollection<StudentRegisterEntry> StudentRegisterEntries { get; set; }
    }
}
