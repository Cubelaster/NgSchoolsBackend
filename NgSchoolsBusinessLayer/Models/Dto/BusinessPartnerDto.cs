using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class BusinessPartnerDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Oib { get; set; }
        public string Address { get; set; }

        public int? CountryId { get; set; }
        public virtual CountryDto Country { get; set; }
        public int? CityId { get; set; }
        public virtual CityDto City { get; set; }
        public int? RegionId { get; set; }
        public virtual RegionDto Region { get; set; }

        public List<ContactPersonDto> BusinessPartnerContacts { get; set; }
    }
}
