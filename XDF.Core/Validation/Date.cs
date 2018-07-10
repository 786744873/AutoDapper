using System.Text.RegularExpressions;
using XDF.Core.Helper.Ajax;
namespace XDF.Core.Validation
{
    /// <summary>
    ///     匹配2017-10-10|2017-10-10 12:12|2017-10-10 12:12:12
    /// </summary>
    public class DateAttribute : ValidationAttribute
    {
        private readonly string _errMsg;
        public DateAttribute(string errMsg = "日期格式不正确")
        {
            _errMsg = errMsg;
        }
        public override AjaxResultModel<T> IsValid<T>(object value)
        {
            if (value != null) {
                var res = Regex.IsMatch(value.ToString(), @"^\d{4}\-\d{2}\-\d{2} \d{2}\:\d{2}\:\d{2}|\d{4}\-\d{2}\-\d{2} \d{2}\:\d{2}|\d{4}\-\d{2}\-\d{2}$");
                if (res) {
                    return AjaxResult.Success<T>(1000, "日期验证成功");
                }
            }
            return AjaxResult.Error<T>(_errMsg);
        }
    }
}