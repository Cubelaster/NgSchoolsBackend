using NgSchoolsDataLayer.Models.BaseTypes;
using System.Collections.Generic;

namespace NgSchoolsDataLayer.Models
{
    public class GoverningCouncil : DatabaseEntity
    {
        public GoverningCouncil() : base() { }

        public int Id { get; set; }
        public string Name { get; set; }
        public int InstitutionId { get; set; }
        public ICollection<GoverningCouncilMember> GoverningCouncilMembers { get; set; }
    }
}
