using System.Collections.Generic;
using Core.Utilities.Attributes;
using NgSchoolsDataLayer.Models.BaseTypes;

namespace NgSchoolsDataLayer.Models
{
    public class StudentRegister : DatabaseEntity
    {
        public int Id { get; set; }
        [Searchable]
        public int BookNumber { get; set; }
        [Searchable]
        public int? BookYear { get; set; }
        public bool Full { get; set; }

        public virtual ICollection<StudentRegisterEntry> StudentRegisterEntries { get; set; }
    }
}
