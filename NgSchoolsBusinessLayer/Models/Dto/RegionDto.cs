using Core.Enums;
using Core.Utilities.Attributes;
using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    [Cached(CacheKeysEnum.Region)]
    public class RegionDto
    {
        public int? Id { get; set; }
        public int? CountryId { get; set; }
        [Searchable]
        public string Region { get; set; }
        [Searchable]
        public string NameDomestic { get; set; }

        public virtual List<CityDto> Cities { get; set; }
    }
}
