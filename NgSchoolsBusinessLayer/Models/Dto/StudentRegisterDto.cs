using NgSchoolsBusinessLayer.Enums;
using NgSchoolsBusinessLayer.Utilities.Attributes;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    [Cached(CacheKeysEnum.StudentRegister)]
    public class StudentRegisterDto
    {
        public int? Id { get; set; }
        [Searchable]
        public int? BookNumber { get; set; }
        public int? NumberOfEntries { get; set; }
        public int? MinEntryNumber { get; set; }
        public int? MaxEntryNumber { get; set; }
    }
}
