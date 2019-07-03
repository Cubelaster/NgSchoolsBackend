using System;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class UserExamCommissionDto
    {
        public int? Id { get; set; }
        public int? ExamCommissionId { get; set; }
        public Guid? UserId { get; set; }
        public UserDetailsDto UserDetails { get; set; }
        public string CommissionRole { get; set; }
    }
}
