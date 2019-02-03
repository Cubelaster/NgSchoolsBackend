using NgSchoolsBusinessLayer.Enums;
using NgSchoolsBusinessLayer.Utilities.Attributes;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    [Cached(CacheKeysEnum.ClassLocation)]
    public class ClassLocationsDto
    {
        public int? Id { get; set; }
        [Searchable]
        public string Name { get; set; }
        [Searchable]
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int? CountryId { get; set; }
        public CountryDto Country { get; set; }
        public int? RegionId { get; set; }
        public RegionDto Region { get; set; }
        public int? CityId { get; set; }
        public CityDto City { get; set; }
    }
}
