using System;

namespace NgSchoolsBusinessLayer.Models.Requests
{
    public class StudentRegisterEntryInsertRequest
    {
        public int? EntryId { get; set; }
        public int? EducationProgramId { get; set; }
        public int? StudentGroupId { get; set; }
        public int? StudentId { get; set; }
        public int? BookNumber { get; set; }
        public int? BookId { get; set; }
        public int? StudentRegisterNumber { get; set; }
        public int? StudentInGroupId { get; set; }
        public bool CreateNewBook { get; set; }
        public string Notes { get; set; }
        public DateTime? ExamDate { get; set; }
        public DateTime? EntryDate { get; set; }
        public int? ExamDateNumber { get; set; }
    }
}
