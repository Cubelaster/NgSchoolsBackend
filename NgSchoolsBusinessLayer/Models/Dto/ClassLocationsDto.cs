using NgSchoolsBusinessLayer.Utilities.Attributes;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class ClassLocationsDto
    {
        public int? Id { get; set; }
        [Searchable]
        public string Name { get; set; }
        [Searchable]
        public string Address { get; set; }
    }
}
