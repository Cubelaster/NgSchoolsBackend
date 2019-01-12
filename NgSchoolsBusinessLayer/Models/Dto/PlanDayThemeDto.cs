using System;
using System.Collections.Generic;
using System.Text;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class PlanDayThemeDto
    {
        public int? Id { get; set; }
        public ThemeDto Theme { get; set; }
        public int PlanDayId { get; set; }
        public double? HoursNumber {get;set;}
    }
}
