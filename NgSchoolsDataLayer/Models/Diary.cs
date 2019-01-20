using NgSchoolsDataLayer.Models.BaseTypes;
using System;
using System.Collections.Generic;

namespace NgSchoolsDataLayer.Models
{
    public class Diary : DatabaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EducationalGroupMark { get; set; }
        public string SchoolYear { get; set; }
        public DateTime EducationalPeriod { get; set; }
        public string Class { get; set; }
        public string EducationProgramType { get; set; }
        public string PerformingWay { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }

        public int? EducationGroupId { get; set; }
        public virtual EducationGroups EducationGroup { get; set; }
        public Guid? EducationLeaderId { get; set; }
        public virtual User EducationLeader { get; set; }
        public Guid? ClassOfficerId { get; set; }
        public virtual User ClassOfficer { get; set; }
        public int? ClassTypeId { get; set; }
        public virtual ClassType ClassType { get; set; }
        public int? TeachingPlaceId { get; set; }
        public virtual ClassLocations TeachingPlace { get; set; }
        public virtual ICollection<StudentGroup> StudentGroups { get; set; }
    }
}
