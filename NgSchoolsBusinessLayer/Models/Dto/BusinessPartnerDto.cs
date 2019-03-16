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

        public bool ApplicationAttendant { get; set; }

        public int? CountryId { get; set; }
        public CountryDto Country { get; set; }
        public int? CityId { get; set; }
        public CityDto City { get; set; }
        public int? RegionId { get; set; }
        public RegionDto Region { get; set; }
        public int? MunicipalityId { get; set; }
        public MunicipalityDto Municipality { get; set; }

        public bool IsBusinessPartner { get; set; }
        public bool IsEmployer { get; set; }

        public List<ContactPersonDto> BusinessPartnerContacts { get; set; }
    }
}
