using NgSchoolsDataLayer.Models.BaseTypes;
using System.Collections.Generic;

namespace NgSchoolsDataLayer.Models
{
    public class Region : DatabaseEntity
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public string Name { get; set; }
        public string NameDomestic { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}
