﻿using System;

namespace Core.Utilities.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheRefreshSource : Attribute
    {
        public Type type { get; set; }
        public CacheRefreshSource(Type type)
        {
            this.type = type;
        }
    }
}
