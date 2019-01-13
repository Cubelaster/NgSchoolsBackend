﻿using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class PlanDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public List<PlanDayDto> PlanDays { get; set; }
    }
}