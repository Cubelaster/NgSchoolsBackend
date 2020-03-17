using NgSchoolsBusinessLayer.Models.Dto.StudentGroup;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsBusinessLayer.Models.Requests.StudentGroup
{
    public class ModifyStudentsInGroupRequest
    {
        [Required]
        public int? Id { get; set; }
        public List<StudentInGroupDto> StudentsInGroup { get; set; }
    }
}
