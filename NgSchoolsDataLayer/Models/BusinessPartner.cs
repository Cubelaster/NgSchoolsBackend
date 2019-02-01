using NgSchoolsDataLayer.Models.BaseTypes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class BusinessPartner : DatabaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Oib { get; set; }
        public string Address { get; set; }

        public virtual ICollection<ContactPerson> BusinessPartnerContacts { get; set; }
    }
}
