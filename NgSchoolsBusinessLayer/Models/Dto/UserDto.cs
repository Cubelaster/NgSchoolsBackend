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
        [Searchable]
        public string Email { get; set; }
        [Searchable]
        public string UserName { get; set; }
        [Searchable]
        public string FirstName { get; set; }
        [Searchable]
        public string LastName { get; set; }
        public List<Claim> Claims { get; set; }
        public List<string> Roles { get; set; }

        public string UserRoles => Roles != null ? string.Join(", ", Roles) : "";
    }
}