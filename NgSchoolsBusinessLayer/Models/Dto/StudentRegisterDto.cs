using System.Collections.Generic;
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
        [Searchable]
        public int? BookYear { get; set; }
        public int? NumberOfEntries { get; set; }
        public int? MinEntryNumber { get; set; }
        public int? MaxEntryNumber { get; set; }
        public bool Full { get; set; }

        public List<int> FreeStudentRegisterNumbers { get; set; }
    }
}
