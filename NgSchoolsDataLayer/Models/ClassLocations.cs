using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class ClassLocations
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
