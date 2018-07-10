using System.Text.RegularExpressions;
using XDF.Core.Helper.Ajax;
namespace XDF.Core.Validation
{
    public class IdCardsAttribute : ValidationAttribute
    {
        private readonly string _errMsg;
        public IdCardsAttribute(string errMsg = "身份证号不正确")
        {
            _errMsg = errMsg;
        }
        public override AjaxResultModel<T> IsValid<T>(object value)
        {
            if(value != null) {
                var id = value.ToString();
                bool res = false;
                if(id.Length == 18) {
                    res = Regex.IsMatch(id, @"(^[1-9]\d{5}(18|19|20)\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$)");
                }
                else if(id.Length == 15) {
                    res = Regex.IsMatch(value.ToString(), @"(^[1-9]\d{5}\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{2}$)");
                }
                if(res) {
                    return AjaxResult.Success<T>(1000, "身份证号验证成功");
                }
            }
            return AjaxResult.Error<T>(_errMsg);
        }
    }
}