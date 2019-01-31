using NgSchoolsBusinessLayer.Enums;
using NgSchoolsBusinessLayer.Utilities.Attributes;
using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    [Cached(CacheKeysEnum.Country)]
    public class CountryDto
    {
        public int? Id { get; set; }
        [Searchable]
        public string Name { get; set; }
        [Searchable]
        public string NameDomestic { get; set; }
        [Searchable]
        public string Alpha2Code { get; set; }
        [Searchable]
        public string Alpha3Code { get; set; }
        [Searchable]
        public int? UnCode { get; set; }
        public int? CountryCallingCode { get; set; }
        public int? InternationalDiallingPrefix { get; set; }

        public virtual List<RegionDto> Regions { get; set; }
        public virtual List<CityDto> Cities { get; set; }
    }
}
