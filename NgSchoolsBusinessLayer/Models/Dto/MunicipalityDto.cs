using Core.Enums;
using Core.Utilities.Attributes;
using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    [Cached(CacheKeysEnum.Municipality)]
    public class MunicipalityDto
    {
        public int Id { get; set; }
        [Searchable]
        public string Name { get; set; }
        public string NameDomestic { get; set; }
        public int? RegionId { get; set; }
        public int CountryId { get; set; }

        public virtual List<CityDto> Cities { get; set; }
    }
}
