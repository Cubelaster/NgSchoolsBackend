using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class ClassType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
