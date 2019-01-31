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
        public string FinishedSchool { get; set; }
        public string ProgramDate { get; set; }
        public double? TheoreticalClassesDuration { get; set; }
        public double? PracticalClassesDuration { get; set; }
        public string ApprovalClass { get; set; }
        public string UrNumber { get; set; }
        public string ComplexityLevel { get; set; }
        public string ProgramJustifiability { get; set; }
        public string EnrollmentConditions { get; set; }
        public string WorkingEnvironment { get; set; }
        public string ProgramCompetencies { get; set; }
        public string PerformingWay { get; set; }
        public string KnoweledgeVerification { get; set; }

        public List<SubjectDto> Subjects { get; set; }
        public PlanDto Plan { get; set; }
    }
}
