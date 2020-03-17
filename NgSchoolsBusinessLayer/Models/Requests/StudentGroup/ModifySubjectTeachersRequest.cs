using NgSchoolsBusinessLayer.Models.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsBusinessLayer.Models.Requests.StudentGroup
{
    public class ModifySubjectTeachersRequest
    {
        [Required]
        public int? Id { get; set; }
        public List<StudentGroupSubjectTeachersDto> SubjectTeachers { get; set; }
    }
}
