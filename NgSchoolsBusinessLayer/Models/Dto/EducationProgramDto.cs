using NgSchoolsBusinessLayer.Enums;
using NgSchoolsBusinessLayer.Utilities.Attributes;
using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    [Cached(CacheKeysEnum.EducationProgram)]
    public class EducationProgramDto
    {
        public int? Id { get; set; }
        [Searchable]
        public string Name { get; set; }
        [Searchable]
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
        public string AgencyProgramDate { get; set; }
        public string AgencyApprovalClass { get; set; }
        public string AgencyUrNumber { get; set; }
        public string RegularClassesTeoretical { get; set; }
        public string RegularClassesPractical { get; set; }
        public string CIClassesGroup { get; set; }
        public string CIClassesIndividual { get; set; }
        public string CIClassesPractical { get; set; }
        public string ProgramType { get; set; }
        public string RegularClassesWorkShop { get; set; }
        public string CIClassesWorkShop { get; set; }

        public List<SubjectDto> Subjects { get; set; }
        public PlanDto Plan { get; set; }
        public List<ClassTypeDto> ClassTypes { get; set; }
        public List<int> ClassTypeIds { get; set; }
        public int? EducationGroupId { get; set; }
        public EducationGroupDto EducationGroup { get; set; }
        public List<FileDto> Files { get; set; }
    }
}
