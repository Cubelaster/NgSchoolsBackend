using NgSchoolsDataLayer.Models.BaseTypes;
using System;

namespace NgSchoolsDataLayer.Models
{
    public class StudentGroupSubjectTeachers : DatabaseEntity
    {
        public int Id { get; set; }
        public int StudentGroupId { get; set; }
        public int SubjectId { get; set; }
        public Guid TeacherId { get; set; }
    }
}
