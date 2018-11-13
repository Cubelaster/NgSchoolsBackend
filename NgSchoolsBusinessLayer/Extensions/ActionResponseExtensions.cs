using NgSchoolsBusinessLayer.Enums.Common;
using NgSchoolsBusinessLayer.Models.Common;

namespace NgSchoolsBusinessLayer.Extensions
{
    public static class ActionResponseExtensions
    {
        public static T GetData<T>(this ActionResponse<T> actionResponse)
        {
            return (T)actionResponse.Data;
        }

        public static bool IsSuccess<T>(this ActionResponse<T> actionResponse, out T Data)
        {
            Data = actionResponse.Data;
            if (actionResponse.ActionResponseType == ActionResponseTypeEnum.Success)
            {
                return true;
            }
            return false;
        }

        public static bool IsSuccessAndHasData<T>(this ActionResponse<T> actionResponse, out T Data)
        {
            Data = actionResponse.Data;
            if (actionResponse.ActionResponseType == ActionResponseTypeEnum.Success && Data != null)
            {
                return true;
            }
            return false;
        }
    }
}
