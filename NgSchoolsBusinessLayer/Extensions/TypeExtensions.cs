using NgSchoolsBusinessLayer.Utilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NgSchoolsBusinessLayer.Extensions
{
    public static class TypeExtensions
    {
        public static List<PropertyInfo> GetSearchableProperties(this Type objectType)
        {
            if (!objectType.FullName.Contains("Dto") && !objectType.FullName.Contains("ViewModel"))
            {
                string typeName = "NgSchoolsBusinessLayer.Models.Dto."
                    + objectType.FullName.Substring(objectType.FullName.LastIndexOf('.') + 1) + "Dto";
                var objectTypeForSearchable = Assembly.GetExecutingAssembly().GetType(typeName);

                var searchableProperties = objectTypeForSearchable.GetProperties()
                    .Where(p => p.GetCustomAttributes().OfType<Searchable>().Any()).ToList();

                var realSearchableProperties = objectType.GetProperties()
                    .Where(p => searchableProperties.Any(sp => sp.Name == p.Name)).ToList();

                return realSearchableProperties;
            }

            if (objectType.FullName.Contains("StudentRegisterEntryDto", StringComparison.OrdinalIgnoreCase))
            {
                var properties = objectType.GetProperties().ToList();

                var searchableProperties = properties
                    .Where(p => p.GetCustomAttributes().OfType<Searchable>().Any()).ToList();

                properties.ForEach(prop =>
                {
                    if (prop.PropertyType.IsClass && prop.PropertyType.Name.Contains("Dto", StringComparison.OrdinalIgnoreCase))
                    {
                        searchableProperties.AddRange(prop.PropertyType.GetSearchableProperties());
                    }
                });

                return searchableProperties;
            }

            return objectType.GetProperties().Where(p => p.GetCustomAttributes().OfType<Searchable>().Any()).ToList();
        }

        public static List<string> GetPropertiesByTypeNameForValueFetch(this Type objectType, PropertyInfo propertyInfo)
        {
            return objectType.GetProperties()
                .Where(p => p.PropertyType.Name == propertyInfo.DeclaringType.Name)
                .Select(pt => $"{pt.Name}.{propertyInfo.Name}")
                .ToList();
        }
    }
}
