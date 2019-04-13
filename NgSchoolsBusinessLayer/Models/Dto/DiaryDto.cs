using NgSchoolsBusinessLayer.Utilities.Attributes;
using System;
using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class DiaryDto
    {
        public int? Id { get; set; }
        [Searchable]
        public string Name { get; set; }
        [Searchable]
        public string EducationalGroupMark { get; set; }
        public string SchoolYear { get; set; }
        public DateTime? EducationalPeriod { get; set; }
        [Searchable]
        public string Class { get; set; }
        public string EducationProgramType { get; set; }
        public string PerformingWay { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }

        public int? EducationGroupId { get; set; }
        public EducationGroupDto EducationGroup { get; set; }
        public Guid? EducationLeaderId { get; set; }
        public UserDto EducationLeader { get; set; }
        public Guid? ClassOfficerId { get; set; }
        public UserDto ClassOfficer { get; set; }
        public int? ClassTypeId { get; set; }
        public ClassTypeDto ClassType { get; set; }
        public int? TeachingPlaceId { get; set; }
        public ClassLocationsDto TeachingPlace { get; set; }
        public List<StudentGroupDto> StudentGroups { get; set; }
        public List<int> StudentGroupIds { get; set; }
    }
}
