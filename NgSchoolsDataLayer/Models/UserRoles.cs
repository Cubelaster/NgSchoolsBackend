using Microsoft.AspNetCore.Identity;
using System;

namespace NgSchoolsDataLayer.Models
{
    public class UserRoles : IdentityUserRole<Guid>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
