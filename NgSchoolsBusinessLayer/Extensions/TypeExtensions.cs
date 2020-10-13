using Core.Utilities.Attributes;
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

        /// <summary>
        /// Gets ONLY NON Object Type <see cref="Searchable"/>
        /// This is because of Audit Data, which is hierarchical, so we get stack overflow because of infinite loop
        /// This only goes 1 tier below
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="existingProps"></param>
        /// <returns></returns>
        public static List<PropertyInfo> GetSearchablePropertiesSingleTier(this Type objectType,
            List<PropertyInfo> existingProps = null)
        {
            var searchableProperties = objectType.GetSearchables()
                .Where(p => !p.PropertyType.IsClass || !p.PropertyType.Name.Contains("Zeraxo.Crm"))
                .Where(p => existingProps == null || !existingProps.Any(ep => ep.Name == p.Name))
                .ToList();

            return searchableProperties;
        }

        /// <summary>
        /// Gets the NAMED Properties that we can use to make query filter from
        /// For instance User.Name
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static List<string> GetPropertiesByTypeNameForValueFetch(this Type objectType, PropertyInfo propertyInfo)
        {
            return objectType.GetSearchables()
                .Where(p => p.PropertyType.Name == propertyInfo.DeclaringType.Name)
                .Select(pt => $"{pt.Name}.{propertyInfo.Name}")
                .ToList();
        }

        public static Dictionary<string, PropertyInfo> GetPropKeyNameValue(this Type objectType, PropertyInfo propertyInfo)
        {
            return objectType.GetSearchables()
                .Where(p => p.PropertyType.Name == propertyInfo.DeclaringType.Name)
                .ToDictionary(pt => $"{pt.Name}.{propertyInfo.Name}", pt => pt);
        }

        /// <summary>
        /// Gets ALL the props that have a <see cref="Searchable"/> attribute.
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static List<PropertyInfo> GetSearchables(this Type objectType)
        {
            return objectType.GetProperties()
                .Where(p => p.GetCustomAttributes().OfType<Searchable>().Any())
                .ToList();
        }
    }
}
