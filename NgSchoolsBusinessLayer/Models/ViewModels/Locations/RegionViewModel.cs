using NgSchoolsBusinessLayer.Utilities.Attributes;

namespace NgSchoolsBusinessLayer.Models.ViewModels.Locations
{
    public class RegionViewModel
    {
        public int? Id { get; set; }
        public int? CountryId { get; set; }
        [Searchable]
        public string Region { get; set; }
        [Searchable]
        public string NameDomestic { get; set; }
    }
}
