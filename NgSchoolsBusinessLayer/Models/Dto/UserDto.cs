using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public List<Claim> Claims { get; set; }
    }
}