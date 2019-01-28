using NgSchoolsDataLayer.Models.BaseTypes;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class StudentFiles : DatabaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        [Required]
        public int FileId { get; set; }
        public virtual UploadedFile File { get; set; }
    }
}
