using NgSchoolsDataLayer.Models.BaseTypes;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class TeacherFile : DatabaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserDetailsId { get; set; }
        public virtual UserDetails UserDetails { get; set; }
        [Required]
        public int FileId { get; set; }
        public virtual UploadedFile File { get; set; }
    }
}
