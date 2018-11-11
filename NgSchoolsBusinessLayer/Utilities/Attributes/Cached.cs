using NgSchoolsBusinessLayer.Enums;
using System;

namespace NgSchoolsBusinessLayer.Utilities.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Cached : Attribute
    {
        public string Key { get; set; }

        public Cached(CacheKeysEnum key)
        {
            this.Key = key.ToString();
        }
    }
}
