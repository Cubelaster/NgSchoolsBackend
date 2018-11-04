using System;

namespace NgSchoolsBusinessLayer.Utilities.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class StringValueAttribute : Attribute
    {
        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }

        public string StringValue { get; private set; }
    }
}
