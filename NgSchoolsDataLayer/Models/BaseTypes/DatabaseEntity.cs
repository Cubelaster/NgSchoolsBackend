using NgSchoolsDataLayer.Enums;
using System;

namespace NgSchoolsDataLayer.Models.BaseTypes
{
    abstract class DatabaseEntity
    {
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DatabaseEntityStatusEnum Status { get; set; }
    }
}
