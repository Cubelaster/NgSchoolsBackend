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
        public Guid? Id { get; set; }
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
        public List<Claim> Claims { get; set; }
        public List<RoleDto> UserRoles { get; set; }
        public List<string> Roles { get; set; }
    }
}