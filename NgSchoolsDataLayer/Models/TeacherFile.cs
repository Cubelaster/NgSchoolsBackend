using NgSchoolsDataLayer.Models.BaseTypes;
using System;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class TeacherFile : DatabaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Guid TeacherId { get; set; }
        public virtual User Teacher { get; set; }
        [Required]
        public int FileId { get; set; }
        public virtual UploadedFile File { get; set; }
    }
}
