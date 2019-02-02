using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class StudentGroupClassAttendanceDto
    {
        public int? Id { get; set; } 
        public int? StudentGroupId { get; set; }
        public string Date { get; set; }
        public List<StudentClassAttendanceDto> StudentClassAttendances { get; set; }
    }
}
