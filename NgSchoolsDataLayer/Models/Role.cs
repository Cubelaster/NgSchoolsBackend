using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace NgSchoolsDataLayer.Models
{
    public class Role : IdentityRole<Guid>
    {
        public ICollection<UserRoles> UserRoles { get; set; }
    }
}
