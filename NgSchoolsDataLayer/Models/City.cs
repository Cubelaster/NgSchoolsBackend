using NgSchoolsDataLayer.Models.BaseTypes;

namespace NgSchoolsDataLayer.Models
{
    public class City : DatabaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameDomestic { get; set; }
        public string Latitude { get; set; }
        public string Longtitude { get; set; }
        public string PoBoxNumber { get; set; }

        public int? RegionId { get; set; }
        public virtual Region Region { get; set; }

        public int? MunicipalityId { get; set; }
        public virtual Municipality Municipality { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
