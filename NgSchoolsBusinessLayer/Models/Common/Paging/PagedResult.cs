using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Common.Paging
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public List<T> Results { get; set; }
    }
}
