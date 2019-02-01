using NgSchoolsDataLayer.Models.BaseTypes;
using System.Collections.Generic;

namespace NgSchoolsDataLayer.Models
{
    public class Country : DatabaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameDomestic { get; set; }
        public string Alpha2Code { get; set; }
        public string Alpha3Code { get; set; }
        public int? UnCode { get; set; }
        public int? CountryCallingCode { get; set; }
        public int? InternationalDiallingPrefix { get; set; }

        public virtual ICollection<Region> Regions { get; set; }
        public virtual ICollection<City> Cities { get; set; }
    }
}
