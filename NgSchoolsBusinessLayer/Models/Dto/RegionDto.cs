using NgSchoolsBusinessLayer.Enums;
using NgSchoolsBusinessLayer.Utilities.Attributes;
using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    [Cached(CacheKeysEnum.Region)]
    public class RegionDto
    {
        public int? Id { get; set; }
        public int? CountryId { get; set; }
        [Searchable]
        public string Name { get; set; }
        [Searchable]
        public string NameDomestic { get; set; }

        public virtual List<CityDto> Cities { get; set; }
    }
}
