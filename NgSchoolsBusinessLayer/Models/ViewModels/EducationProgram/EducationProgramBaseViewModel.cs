using Core.Utilities.Attributes;

namespace NgSchoolsBusinessLayer.Models.ViewModels.EducationProgram
{
    public class EducationProgramBaseViewModel
    {
        public int? Id { get; set; }
        [Searchable]
        public string Name { get; set; }
        [Searchable]
        public string ShorthandName { get; set; }
    }
}
