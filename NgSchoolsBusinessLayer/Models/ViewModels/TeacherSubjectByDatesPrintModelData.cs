using NgSchoolsBusinessLayer.Models.Dto;
using System;
using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.ViewModels
{
    public class TeacherSubjectByDatesPrintModelData
    {
        public UserDetailsDto Teacher { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public SubjectDto Subject { get; set; }
        public List<ThemeDto> Themes { get; set; }
    }
}
