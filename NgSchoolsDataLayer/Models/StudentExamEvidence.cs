using System;

namespace NgSchoolsDataLayer.Models
{
    public class StudentExamEvidence
    {
        public int Id { get; set; }
        public DateTime ExamDate { get; set; }
        public string ExamEvidence { get; set; }

        public int StudentsInGroupsId { get; set; }
        public virtual StudentsInGroups StudentsInGroups { get; set; }
    }
}
