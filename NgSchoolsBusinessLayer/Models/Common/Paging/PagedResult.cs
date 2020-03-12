using System.Collections.Generic;
using System.Linq;

namespace NgSchoolsBusinessLayer.Models.Common.Paging
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public List<T> Results { get; set; }
        public IQueryable<T> ResultQuery { get; set; }
    }
}
