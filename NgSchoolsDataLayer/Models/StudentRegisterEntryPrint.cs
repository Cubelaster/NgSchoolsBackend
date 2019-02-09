using NgSchoolsDataLayer.Models.BaseTypes;
using System;

namespace NgSchoolsDataLayer.Models
{
    public class StudentRegisterEntryPrint : DatabaseEntity
    {
        public int Id { get; set; }
        public DateTime PrintDate { get; set; }

        public int StudentRegisterEntryId { get; set; }
        public virtual StudentRegisterEntry StudentRegisterEntry { get; set; }
    }
}
