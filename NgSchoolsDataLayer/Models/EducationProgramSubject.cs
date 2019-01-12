using NgSchoolsDataLayer.Models.BaseTypes;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class EducationProgramSubject : DatabaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
        [Required]
        public int EducationProgramId { get; set; }
        public virtual EducationProgram EducationProgram { get; set; }
    }
}
