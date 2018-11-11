using System;

namespace NgSchoolsBusinessLayer.Models.Responses
{
    public class GenerateJwtTokenResponse
    {
        public string Token { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}
