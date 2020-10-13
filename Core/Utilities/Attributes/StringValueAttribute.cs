using System;

namespace Core.Utilities.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class StringValueAttribute : Attribute
    {
        public StringValueAttribute(string value)
        {
            StringValue = value;
        }

        public string StringValue { get; private set; }
    }
}
