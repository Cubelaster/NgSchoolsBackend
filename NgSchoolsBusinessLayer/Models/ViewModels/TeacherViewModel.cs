using System;

namespace NgSchoolsBusinessLayer.Models.ViewModels
{
    public class TeacherViewModel
    {
        public Guid? UserId { get; set; }
        public string Oib { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Profession { get; set; }
        public string Qualifications { get; set; }
        public string EmploymentPlace { get; set; }
        public string Bank { get; set; }
        public string Iban { get; set; }
        public bool PpEducation { get; set; }
        public string Authorization { get; set; }
        public string Certificates { get; set; }
        public string Notes { get; set; }
    }
}
