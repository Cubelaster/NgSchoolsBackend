using System;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class StudentGroupSubjectTeachersDto
    {
        public int? Id { get; set; }
        public int? StudentGroupId { get; set; }
        public int? SubjectId { get; set; }
        public Guid? TeacherId { get; set; }
    }
}
