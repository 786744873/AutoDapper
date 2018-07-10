using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace XDF.Core.Helper.Ajax
{
    [Serializable]
    public class AjaxResultModel<T>
    {
        public AjaxResultModel()
        {
            IsSuccess = false;
            Msg = "";
            Data = default(T);
            Code = -1;
        }
        public AjaxResultModel(int code, string msg, T data, bool isSuccess)
        {
            Code = code;
            Msg = msg;
            Data = data;
            IsSuccess = isSuccess;
        }
        public override string ToString()
        {
            // return new JavaScriptSerializer().Serialize(this);
            var timeConverter = new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm"
            };
            var settings = new JsonSerializerSettings
            {
                //这句是解决问题的关键,也就是json.net官方给出的解决配置选项.                 
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };
            return JsonConvert.SerializeObject(this, Formatting.None, timeConverter);
        }
        #region  Model
        public int Code { get; set; }
        public string Msg { get; set; }
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        #endregion
    }
}