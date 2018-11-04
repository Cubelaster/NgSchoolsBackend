using NgSchoolsBusinessLayer.Enums.Common;

namespace NgSchoolsBusinessLayer.Models.Common
{
    public class ActionResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public ActionResponseTypeEnum ActionResponseType { get; set; }

        public static ActionResponse<T> ReturnSuccess(object Data = null, string Message = null)
        {
            return new ActionResponse<T>()
            {
                Data = (T)Data,
                Message = Message,
                ActionResponseType = ActionResponseTypeEnum.Success
            };
        }
    }
}
