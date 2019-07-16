using NgSchoolsBusinessLayer.Models.Dto;
using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.ViewModels
{
    public class StudentEducationProgramsPrintModel : StudentDto
    {
        public List<EducationProgramDto> EducationPrograms { get; set; }
    }
}
