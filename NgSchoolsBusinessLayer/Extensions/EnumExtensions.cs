using Core.Utilities.Attributes;
using System;
using System.Reflection;

namespace NgSchoolsBusinessLayer.Extensions
{
    public static class EnumExtensions
    {
        public static string GetStringValue(this Enum @enum)
        {
            Type type = @enum.GetType();
            FieldInfo fi = type.GetField(@enum.ToString());
            StringValueAttribute[] attrs =
                fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
            if (attrs.Length > 0)
            {
                return attrs[0].StringValue;
            }
            return null;
        }
    }
}
