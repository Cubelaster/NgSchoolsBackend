using NgSchoolsBusinessLayer.Models.ViewModels.Locations;

namespace NgSchoolsBusinessLayer.Models.Common
{
    public abstract class LocationsHolder
    {
        public int? CountryId { get; set; }
        public CountryViewModel Country { get; set; }

        public int? RegionId { get; set; }
        public RegionViewModel Region { get; set; }

        public int? MunicipalityId { get; set; }
        public MunicipalityViewModel Municipality { get; set; }

        public int? CityId { get; set; }
        public CityViewModel City { get; set; }
    }
}
