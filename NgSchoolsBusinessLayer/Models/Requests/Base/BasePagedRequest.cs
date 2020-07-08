using Newtonsoft.Json.Linq;
using NgSchoolsBusinessLayer.Enums.Common;

namespace NgSchoolsBusinessLayer.Models.Requests.Base
{
    public class BasePagedRequest
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string SearchQuery { get; set; }
        public JObject Where { get; set; }
        public string OrderBy { get; set; }
        public SortDirectionEnum SortDirection { get; set; }
        public SimpleRequestBase AdditionalParams { get; set; }
    }
}
