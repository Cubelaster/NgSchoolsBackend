using NgSchoolsDataLayer.Models.BaseTypes;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class InstitutionFile : DatabaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int InstitutionId { get; set; }
        public virtual Institution Student { get; set; }
        [Required]
        public int FileId { get; set; }
        public virtual UploadedFile File { get; set; }
    }
}
