using NgSchoolsDataLayer.Models.BaseTypes;
using System.Collections.Generic;

namespace NgSchoolsDataLayer.Models
{
    public class StudentRegister : DatabaseEntity
    {
        public int Id { get; set; }
        public int BookNumber { get; set; }

        public virtual ICollection<StudentRegisterEntry> StudentRegisterEntries { get; set; }
    }
}
