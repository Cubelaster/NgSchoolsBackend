using NgSchoolsDataLayer.Models.BaseTypes;
using System;
using System.Collections.Generic;

namespace NgSchoolsDataLayer.Models
{
    public class StudentGroupClassAttendance : DatabaseEntity
    {
        public int Id { get; set; }
        public int StudentGroupId { get; set; }
        public virtual StudentGroup StudentGroup { get; set; }
        public DateTime Date { get; set; }

        public ICollection<StudentClassAttendance> StudentClassAttendances { get; set; }
    }
}
