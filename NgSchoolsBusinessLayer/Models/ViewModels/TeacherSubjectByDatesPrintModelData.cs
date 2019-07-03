using NgSchoolsBusinessLayer.Models.Dto;
using System;

namespace NgSchoolsBusinessLayer.Models.ViewModels
{
    public class TeacherSubjectByDatesPrintModelData
    {
        public UserDetailsDto Teacher { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public SubjectDto Subject { get; set; }
    }
}
