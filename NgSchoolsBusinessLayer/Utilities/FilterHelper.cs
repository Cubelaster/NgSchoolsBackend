using NgSchoolsBusinessLayer.Models.Filters;
using NgSchoolsDataLayer.Models;
using System;

namespace NgSchoolsBusinessLayer.Utilities
{
    public static class FilterHelper
    {
        public static Type GetMatchingType<T>() where T : class, new()
        {
            var typedObject = new T();

            switch (typedObject)
            {
                case EducationProgram _:
                    return typeof(EducationProgramFilterObject);
                default:
                    return typeof(T);
            }
        }
    }
}
