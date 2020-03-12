namespace NgSchoolsBusinessLayer.Models.Dto.StudentGroup
{
    public class StudentInGroupBaseDto
    {
        public int? Id { get; set; }
        public int? StudentId { get; set; }
        public int? GroupId { get; set; }
        public bool CompletedPractice { get; set; }
        public int? EmployerId { get; set; }
        public string PracticalStartDate { get; set; }
        public string PracticalEndDate { get; set; }
        public int? StudentRegisterNumber { get; set; }
    }
}
