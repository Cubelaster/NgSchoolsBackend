using NgSchoolsBusinessLayer.Enums;
using NgSchoolsBusinessLayer.Utilities.Attributes;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    [Cached(CacheKeysEnum.User)]
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Claim> Claims { get; set; }

        public string UserRoles => Claims != null ? string.Join(", ", Claims) : "";
    }
}