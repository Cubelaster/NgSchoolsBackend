using Microsoft.AspNetCore.Identity;
using NgSchoolsDataLayer.Enums;
using System;
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
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DatabaseEntityStatusEnum Status { get; set; }
    }
}
