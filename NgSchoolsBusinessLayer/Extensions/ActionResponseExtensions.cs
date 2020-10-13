using Core.Enums.Common;
using NgSchoolsBusinessLayer.Models.Common;
using System;

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

        public static bool IsNotSuccess<T>(this ActionResponse<T> response, out ActionResponse<T> actionResponse, out T Data)
        {
            actionResponse = response;
            Data = actionResponse.Data != null ? (T)actionResponse.Data : default(T);
            if (response.ActionResponseType != ActionResponseTypeEnum.Success)
            {
                return true;
            }
            return false;
        }

        public static bool IsNotSuccess<T>(this ActionResponse<T> response)
        {
            if (response.ActionResponseType != ActionResponseTypeEnum.Success)
            {
                return true;
            }
            return false;
        }

        public static bool IsNotSuccess<T>(this ActionResponse<T> response, out ActionResponse<T> actionResponse)
        {
            actionResponse = response;
            if (response.ActionResponseType != ActionResponseTypeEnum.Success)
            {
                return true;
            }
            return false;
        }

        public static ActionResponse<T> AppendErrorMessage<T>(this ActionResponse<T> response, string errorMessage)
        {
            response.ActionResponseType = ActionResponseTypeEnum.Error;
            response.Message += string.IsNullOrEmpty(response.Message)
                || response.Message.EndsWith(Environment.NewLine)
                ? errorMessage : Environment.NewLine + errorMessage;
            return response;
        }

        public static ActionResponse<T> AppendMessage<T>(this ActionResponse<T> response, string message)
        {
            response.Message += string.IsNullOrEmpty(response.Message)
                || response.Message.EndsWith(Environment.NewLine)
                ? message : Environment.NewLine + message;
            return response;
        }
    }
}
