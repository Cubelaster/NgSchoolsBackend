using System;

namespace NgSchoolsBusinessLayer.Models.Requests.Base
{
    public class SimpleRequestBase
    {
        public int Id { get; set; }
        public string SearchParam { get; set; }
        public DateTime? DateParam { get; set; }
    }
}
