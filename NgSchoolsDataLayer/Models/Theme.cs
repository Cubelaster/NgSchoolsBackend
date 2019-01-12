﻿using NgSchoolsDataLayer.Models.BaseTypes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class Theme : DatabaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Content { get; set; }
        public string LearningOutcomes { get; set; }

        public virtual ICollection<SubjectTheme> ThemeSubjects { get; set; }
    }
}
