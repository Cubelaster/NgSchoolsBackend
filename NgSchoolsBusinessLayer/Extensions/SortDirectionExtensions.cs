using Core.Enums.Common;

namespace NgSchoolsBusinessLayer.Extensions
{
    public static class SortDirectionExtensions
    {
        public static string GetOrderByNames(this SortDirectionEnum value, bool isThen = false)
        {
            switch (value)
            {
                case SortDirectionEnum.ASC:
                    return isThen ? "ThenBy" : "OrderBy";
                default:
                    return isThen ? "ThenByDescending" : "OrderByDescending";
            }
        }
    }
}
