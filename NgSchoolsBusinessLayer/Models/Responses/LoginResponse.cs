using System;

namespace NgSchoolsBusinessLayer.Models.Responses
{
    public class LoginResponse
    {
        public Guid UserId { get; set; }
        public string JwtToken { get; set; }
    }
}
