using NgSchoolsBusinessLayer.Enums;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Utilities.Attributes;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    [Cached(CacheKeysEnum.ClassLocation)]
    public class ClassLocationsDto : LocationsHolder
    {
        public int? Id { get; set; }
        [Searchable]
        public string Name { get; set; }
        [Searchable]
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
    }
}
