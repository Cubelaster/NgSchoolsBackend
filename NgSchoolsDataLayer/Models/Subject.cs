﻿using System.Collections.Generic;

namespace NgSchoolsDataLayer.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubjectCompetence { get; set; }
        public string WorkMethods { get; set; }
        public string MaterialConditions { get; set; }
        public string StaffingConditions { get; set; }
        public string Literature { get; set; }

        public virtual ICollection<SubjectTheme> SubjectThemes { get; set; }
    }
}
