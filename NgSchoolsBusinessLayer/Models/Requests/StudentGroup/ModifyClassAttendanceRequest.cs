using NgSchoolsBusinessLayer.Models.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsBusinessLayer.Models.Requests.StudentGroup
{
    public class ModifyClassAttendanceRequest
    {
        [Required]
        public int? Id { get; set; }
        public List<StudentGroupClassAttendanceDto> StudentGroupClassAttendances { get; set; }
    }
}
