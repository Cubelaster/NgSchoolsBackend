using NgSchoolsDataLayer.Models.BaseTypes;

namespace NgSchoolsDataLayer.Models
{
    public class ContactPerson : DatabaseEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int BusinessPartnerId { get; set; }
        public virtual BusinessPartner BusinessPartner { get; set; }
    }
}
