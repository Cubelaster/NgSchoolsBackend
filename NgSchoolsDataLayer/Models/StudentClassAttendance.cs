using NgSchoolsDataLayer.Models.BaseTypes;

namespace NgSchoolsDataLayer.Models
{
    public class StudentClassAttendance : DatabaseEntity
    {
        public int Id { get; set; }
        public bool Attendance { get; set; }
        public int StudentGroupClassAttendanceId { get; set; }
        public virtual StudentGroupClassAttendance StudentGroupClassAttendance { get; set; }
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
    }
}
