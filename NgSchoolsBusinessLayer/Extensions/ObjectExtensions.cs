using System;
using System.Reflection;

namespace NgSchoolsBusinessLayer.Extensions
{
    public static class ObjectExtensions
    {
        public static object GetPropValue(this object entity, string propName)
        {
            string[] nameParts = propName.Split('.');
            if (nameParts.Length == 1)
            {
                return entity.GetType().GetProperty(propName).GetValue(entity, null);
            }

            foreach (string part in nameParts)
            {
                if (entity == null) { return null; }

                Type type = entity.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                entity = info.GetValue(entity, null);
            }
            return entity;
        }
    }
}
