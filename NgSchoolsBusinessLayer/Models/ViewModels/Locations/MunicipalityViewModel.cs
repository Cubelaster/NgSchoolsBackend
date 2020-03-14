using NgSchoolsBusinessLayer.Utilities.Attributes;

namespace NgSchoolsBusinessLayer.Models.ViewModels.Locations
{
    public class MunicipalityViewModel
    {
        public int Id { get; set; }
        [Searchable]
        public string Name { get; set; }
        public string NameDomestic { get; set; }
        public int? RegionId { get; set; }
        public int CountryId { get; set; }
    }
}
