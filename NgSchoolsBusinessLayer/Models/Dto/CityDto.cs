using Core.Enums;
using Core.Utilities.Attributes;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    [Cached(CacheKeysEnum.City)]
    public class CityDto
    {
        public int? Id { get; set; }
        [Searchable]
        public string City { get; set; }
        [Searchable]
        public string NameDomestic { get; set; }
        public string Latitude { get; set; }
        public string Longtitude { get; set; }
        public string PoBoxNumber { get; set; }
        public int? RegionId { get; set; }
        public int? MunicipalityId { get; set; }
        public int? CountryId { get; set; }
    }
}
