using NgSchoolsDataLayer.Models.BaseTypes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class Municipality : DatabaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameDomestic { get; set; }

        public int? RegionId { get; set; }
        public virtual Region Region { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}
