using Core.Enums.Common;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Models.Common
{
    public class ActionResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public ActionResponseTypeEnum ActionResponseType { get; set; }

        public async static Task<ActionResponse<T>> ReturnSuccess(object Data = null, string Message = null)
        {
            return await Task.FromResult(new ActionResponse<T>
            {
                Data = (T)Data,
                Message = Message,
                ActionResponseType = ActionResponseTypeEnum.Success
            });
        }

        public async static Task<ActionResponse<T>> ReturnError(string Message = null, object Data = null)
        {
            return await Task.FromResult(new ActionResponse<T>
            {
                Data = (T)Data,
                Message = Message,
                ActionResponseType = ActionResponseTypeEnum.Error
            });
        }

        public async static Task<ActionResponse<T>> ReturnWarning(string Message = null, object Data = null)
        {
            return await Task.FromResult(new ActionResponse<T>
            {
                Data = (T)Data,
                Message = Message,
                ActionResponseType = ActionResponseTypeEnum.Warning
            });
        }
    }
}
