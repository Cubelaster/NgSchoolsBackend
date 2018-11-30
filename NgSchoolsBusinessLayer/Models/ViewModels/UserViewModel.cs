using NgSchoolsBusinessLayer.Utilities.Attributes;
using System;
using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.ViewModels
{
    public class UserViewModel
    {
        public Guid? Id { get; set; }
        public int? UserDetailsId { get; set; }
        [Searchable]
        public string Email { get; set; }
        [Searchable]
        public string UserName { get; set; }
        [Searchable]
        public string FirstName { get; set; }
        [Searchable]
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public string Signature { get; set; }
        public string Title { get; set; }
        public string Mobile { get; set; }
        public string Mobile2 { get; set; }
        public string Phone { get; set; }
        public string RoleNames { get; set; }
        public List<string> RolesNamed { get; set; }
        public List<Guid> Roles { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
