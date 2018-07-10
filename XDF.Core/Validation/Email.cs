using System.Text.RegularExpressions;
using XDF.Core.Helper.Ajax;
namespace XDF.Core.Validation
{
    public class EmailAttribute : ValidationAttribute
    {
        private readonly string _errMsg;
        public EmailAttribute(string errMsg = "邮箱不正确")
        {
            _errMsg = errMsg;
        }
        public override AjaxResultModel<T> IsValid<T>(object value)
        {
            if (value != null) {
                var res = Regex.IsMatch(value.ToString(), @"^(\w)+(\.\w+)*@(\w)+((\.\w{2,3}){1,3})$");
                if (res) {
                    return AjaxResult.Success<T>(1000, "验证成功");
                }
            }
            return AjaxResult.Error<T>(_errMsg);
        }
    }
}