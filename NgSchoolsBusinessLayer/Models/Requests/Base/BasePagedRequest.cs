using Newtonsoft.Json.Linq;
using Core.Enums.Common;
using System.Collections.Generic;

namespace NgSchoolsBusinessLayer.Models.Requests.Base
{
    public class BasePagedRequest
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string SearchQuery { get; set; }
        public JObject Where { get; set; }
        public Dictionary<string, SortDirectionEnum> Sorting { get; set; }
        public SimpleRequestBase AdditionalParams { get; set; }
    }
}
