using NgSchoolsBusinessLayer.Utilities.Attributes;

namespace NgSchoolsBusinessLayer.Models.ViewModels.Locations
{
    public class CountryViewModel
    {
        public int? Id { get; set; }
        [Searchable]
        public string Name { get; set; }
        [Searchable]
        public string NameDomestic { get; set; }
        [Searchable]
        public string Code { get; set; }
        [Searchable]
        public string Alpha3Code { get; set; }
        [Searchable]
        public int? UnCode { get; set; }
        public int? CountryCallingCode { get; set; }
        public int? InternationalDiallingPrefix { get; set; }
    }
}
