namespace NgSchoolsBusinessLayer.Models.Requests.Base
{
    public class BasePagedRequest
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string SearchQuery { get; set; }
        public object Where { get; set; }
        public string OrderBy { get; set; }
    }
}
