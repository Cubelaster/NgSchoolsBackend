using System;

namespace NgSchoolsBusinessLayer.Models.ViewModels
{
    public class StudentRegisterEntryGridViewModel
    {
        public int Id { get; set; }
        public DateTime? EntryDate { get; set; }
        public int StudentRegisterId { get; set; }
        public int StudentRegisterNumber { get; set; }
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int EducationProgramId { get; set; }
        public string EducationProgramName { get; set; }
    }
}
