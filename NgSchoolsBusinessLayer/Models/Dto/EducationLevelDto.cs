using NgSchoolsBusinessLayer.Utilities.Attributes;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class EducationLevelDto
    {
        public int? Id { get; set; }
        [Searchable]
        public string Name { get; set; }
        [Searchable]
        public string Level { get; set; }
        public string KnowledgeBase { get; set; }
        public string CognitiveSkills { get; set; }
        public string PsychomotorSkills { get; set; }
        public string SocialSkills { get; set; }
        public string Autonomy { get; set; }
        public string Responsibility { get; set; }
    }
}
