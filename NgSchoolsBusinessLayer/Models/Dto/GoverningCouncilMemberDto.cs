using System;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class GoverningCouncilMemberDto
    {
        public int? Id { get; set; }
        public int? GoverningCouncilId { get; set; }
        public Guid? UserId { get; set; }
        public UserDto User { get; set; }
        public string Role { get; set; }
    }
}
