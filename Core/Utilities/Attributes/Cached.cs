using Core.Enums;
using System;

namespace Core.Utilities.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Cached : Attribute
    {
        public string Key { get; set; }

        public Cached(CacheKeysEnum key)
        {
            Key = key.ToString();
        }
    }
}
