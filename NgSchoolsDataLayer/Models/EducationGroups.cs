using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class EducationGroups
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
