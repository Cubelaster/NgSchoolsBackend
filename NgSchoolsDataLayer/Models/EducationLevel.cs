using NgSchoolsDataLayer.Models.BaseTypes;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class EducationLevel : DatabaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Level { get; set; }
        public string KnowledgeBase { get; set; }
        public string CognitiveSkills { get; set; }
        public string PsychomotorSkills { get; set; }
        public string SocialSkills { get; set; }
        public string Autonomy { get; set; }
        public string Responsibility { get; set; }
    }
}
