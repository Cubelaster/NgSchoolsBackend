namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class StudentInGroupDto
    {
        public int? Id { get; set; }
        public int? StudentId { get; set; }
        public int? GroupId { get; set; }
        public bool CompletedPractice { get; set; }
        public int? EmployerId { get; set; }
        public BusinessPartnerDto Employer { get; set; }

        public string PracticalStartDate { get; set; }
        public string PracticalEndDate { get; set; }
    }
}
