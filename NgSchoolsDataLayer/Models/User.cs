using Microsoft.AspNetCore.Identity;
using NgSchoolsDataLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class User : IdentityUser<Guid>
    {
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public string Signature { get; set; }
        public string Title { get; set; }
        public string Mobile { get; set; }
        public string Mobile2 { get; set; }
        public string Phone { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DatabaseEntityStatusEnum Status { get; set; }

        public ICollection<UserRoles> Roles { get; set; }
    }
}
