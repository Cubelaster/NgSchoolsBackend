using NgSchoolsBusinessLayer.Utilities.Attributes;
using System;
using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.ViewModels
{
    public class TeacherViewModel
    {
        [Searchable]
        public string Email { get; set; }
        [Searchable]
        public string FirstName { get; set; }
        [Searchable]
        public string LastName { get; set; }
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
        public string RoleNames { get; set; }
        public List<string> RolesNamed { get; set; }
        public List<Guid> Roles { get; set; }
    }
}
