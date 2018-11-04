using NgSchoolsBusinessLayer.Models.Common;

namespace NgSchoolsBusinessLayer.Extensions
{
    public static class ActionResponseExtensions
    {
        public static T GetData<T>(this ActionResponse<T> actionResponse)
        {
            return (T)actionResponse.Data;
        }
    }
}
