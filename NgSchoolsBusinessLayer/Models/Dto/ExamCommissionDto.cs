using NgSchoolsBusinessLayer.Utilities.Attributes;
using System;
using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class ExamCommissionDto
    {
        public int? Id { get; set; }
        [Searchable]
        public string Name { get; set; }
        public List<Guid> TeacherIds { get; set; }
        public List<string> TeacherName { get; set; }
        public string Teachers { get; set; }
    }
}
