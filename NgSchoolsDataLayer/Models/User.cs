using Microsoft.AspNetCore.Identity;
using NgSchoolsDataLayer.Enums;
using System;
using System.Collections.Generic;

namespace NgSchoolsDataLayer.Models
{
    public class User : IdentityUser<Guid>
    {
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DatabaseEntityStatusEnum Status { get; set; }

        public UserDetails UserDetails { get; set; }
        public ICollection<UserRoles> Roles { get; set; }
    }
}
