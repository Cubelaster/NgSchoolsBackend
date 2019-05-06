using NgSchoolsDataLayer.Models.BaseTypes;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class EducationProgramFile : DatabaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int EducationProgramId { get; set; }
        public virtual EducationProgram EducationProgram { get; set; }
        [Required]
        public int FileId { get; set; }
        public virtual UploadedFile File { get; set; }
    }
}
