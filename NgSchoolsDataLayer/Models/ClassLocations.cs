using NgSchoolsDataLayer.Models.BaseTypes;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class ClassLocations : DatabaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public int RegionId { get; set; }
        public virtual Region Region { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
    }
}
