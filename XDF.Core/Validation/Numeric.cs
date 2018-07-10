using System.Text.RegularExpressions;
using XDF.Core.Helper.Ajax;
namespace XDF.Core.Validation
{
    public class NumericAttribute : ValidationAttribute
    {
        private readonly string _errMsg;
        public NumericAttribute(string errMsg = "不全是数字")
        {
            _errMsg = errMsg;
        }
        public override AjaxResultModel<T> IsValid<T>(object value)
        {
            if (value != null) {
                var res = Regex.IsMatch(value.ToString(), @"^(-?\d+)(\.\d+)?");
                if (res) {
                    return AjaxResult.Success<T>(1000, "数字验证成功");
                }
            }
            return AjaxResult.Error<T>(_errMsg);
        }
    }
}