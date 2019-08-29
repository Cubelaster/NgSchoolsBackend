using NgSchoolsBusinessLayer.Models.ViewModels;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class StudentRegisterPrintDataAggregatedDto
    {
        public StudentRegisterEntryDto StudentRegisterEntry { get; set; }
        public StudentEducationProgramsPrintModel StudentEducationPrograms { get; set; }
    }
}
