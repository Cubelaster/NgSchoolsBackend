using Core.Utilities.Attributes;
using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class CombinedGroupDto
    {
        public int? Id { get; set; }
        [Searchable]
        public string Name { get; set; }
        public List<StudentGroupDto> StudentGroups { get; set; }
        public List<int> StudentGroupIds { get; set; }
    }
}
