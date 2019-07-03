using NgSchoolsBusinessLayer.Models.Dto;
using System;

namespace NgSchoolsBusinessLayer.Models.ViewModels
{
    public class TeacherSubjectByDatesPrintModel
    {
        public UserDetailsDto Teacher { get; set; }
        public DateTime Date { get; set; }
        public SubjectDto Subject { get; set; }
    }
}
