using NgSchoolsBusinessLayer.Models.Dto;
using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.ViewModels
{
    public class ThemesByWeekPrintModel
    {
        public int Week { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public List<PlanDaySubjectThemeDto> Themes { get; set; }
    }
}
