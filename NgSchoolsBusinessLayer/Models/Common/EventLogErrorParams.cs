using System;

namespace NgSchoolsBusinessLayer.Models.Common
{
    public class EventLogErrorParams
    {
        public string CallerMember { get; set; }
        public string Source { get; set; }
        public int LineNumber { get; set; }
        public string Parameters { get; set; }
        public Exception Exception { get; set; }
    }
}
