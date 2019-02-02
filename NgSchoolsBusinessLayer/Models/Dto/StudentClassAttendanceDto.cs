namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class StudentClassAttendanceDto
    {
        public int? Id { get; set; }
        public int? StudentGroupClassAttendanceId { get; set; }
        public int? StudentId { get; set; }
        public bool Attendance { get; set; }
    }
}
