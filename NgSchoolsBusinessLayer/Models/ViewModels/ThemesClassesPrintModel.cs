using NgSchoolsBusinessLayer.Models.Dto;
using System;
using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.ViewModels
{
    public class ThemesClassesPrintModel
    {
        public DateTime Date { get; set; }
        public List<PlanDayThemeAndTeacherDto> DayClasses { get; set; }
    }
}
