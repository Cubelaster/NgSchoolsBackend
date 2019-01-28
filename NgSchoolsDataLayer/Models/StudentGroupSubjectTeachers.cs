using System;
using System.Collections.Generic;
using System.Text;

namespace NgSchoolsDataLayer.Models
{
    public class StudentGroupSubjectTeachers
    {
        public int Id { get; set; }
        public int StudentGroupId { get; set; }
        public int SubjectId { get; set; }
        public Guid TeacherId { get; set; }
    }
}
