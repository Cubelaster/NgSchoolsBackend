using NgSchoolsDataLayer.Models.BaseTypes;
using System;
using System.Collections.Generic;

namespace NgSchoolsDataLayer.Models
{
    public class EducationProgram : DatabaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShorthandName { get; set; }
        public double? ProgramDuration { get; set; }
        public string ProgramDurationTextual { get; set; }
        public double? ProgramDurationDays { get; set; }
        public string ProgramDate { get; set; }
        public string ApprovalClass { get; set; }
        public string UrNumber { get; set; }
        public string ComplexityLevel { get; set; }
        public string ProgramJustifiability { get; set; }
        public string EnrollmentConditions { get; set; }
        public string WorkingEnvironment { get; set; }
        public string ProgramCompetencies { get; set; }
        public string PerformingWay { get; set; }
        public string KnoweledgeVerification { get; set; }

        public DateTime? AgencyProgramDate { get; set; }
        public string AgencyApprovalClass { get; set; }
        public string AgencyUrNumber { get; set; }
        public string RegularClassesTeoretical { get; set; }
        public string RegularClassesPractical { get; set; }
        public string CIClassesGroup { get; set; }
        public string CIClassesIndividual { get; set; }
        public string CIClassesPractical { get; set; }
        public string CIClassesTheoretical { get; set; }
        public string ProgramType { get; set; }
        public string RegularClassesWorkShop { get; set; }
        public string CIClassesWorkShop { get; set; }
        public string Version { get; set; }

        public int? EducationGroupId { get; set; }
        public virtual EducationGroups EducationGroup { get; set; }

        public ICollection<Subject> Subjects { get; set; }

        public virtual Plan Plan { get; set; }

        public virtual ICollection<EducationProgramClassType> EducationProgramClassTypes { get; set; }
        public virtual ICollection<EducationProgramFile> Files { get; set; }
    }
}
