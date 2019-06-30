using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class GoverningCouncilDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? InstitutionId { get; set; }
        public List<GoverningCouncilMemberDto> GoverningCouncilMembers { get; set; }
    }
}
