﻿using Core.Enums;
using Core.Enums;
using Core.Utilities.Attributes;

namespace NgSchoolsBusinessLayer.Models.Dto
{
    [Cached(CacheKeysEnum.ClassType)]
    public class ClassTypeDto
    {
        public int? Id { get; set; }
        [Searchable]
        public string Name { get; set; }
    }
}
