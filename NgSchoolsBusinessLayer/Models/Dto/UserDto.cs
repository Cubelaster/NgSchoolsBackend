using Core.Enums;
using Core.Utilities.Attributes;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    [Cached(CacheKeysEnum.User)]
    public class UserDto
    {
        public Guid? Id { get; set; }
        public int? UserDetailsId { get; set; }
        public UserDetailsDto UserDetails { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public List<Claim> Claims { get; set; }
        public List<RoleDto> UserRoles { get; set; }
    }
}