using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class StudentsInGroups
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        [Required]
        public int StudentGroupId { get; set; }
        public virtual StudentGroup StudentGroup { get; set; }
    }
}
