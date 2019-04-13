using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class CombinedGroupDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public List<StudentGroupDto> StudentGroups { get; set; }
        public List<int> StudentGroupIds { get; set; }
    }
}
