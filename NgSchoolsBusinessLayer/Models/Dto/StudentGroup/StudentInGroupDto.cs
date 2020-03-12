using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto.StudentGroup
{
    public class StudentInGroupDto : StudentInGroupBaseDto
    {
        public BusinessPartnerDto Employer { get; set; }

        public List<StudentExamEvidenceDto> StudentExamEvidences { get; set; }
    }
}
