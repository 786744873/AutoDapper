using System.Text.RegularExpressions;
using XDF.Core.Helper.Ajax;
namespace XDF.Core.Validation
{
    public class PassWordAttribute : ValidationAttribute
    {
        private readonly string _errMsg;
        public PassWordAttribute(string errMsg = "密码不正确")
        {
            _errMsg = errMsg;
        }
        public override AjaxResultModel<T> IsValid<T>(object value)
        {
            if (value != null) {
                //^(?![\d]+$)(?![a-zA-Z]+$)(?![^\da-zA-Z]+$).{6,20}$  ^(?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9]{6,20}$
                var res = Regex.IsMatch(value.ToString(), @"^(?![\d]+$)(?![a-zA-Z]+$)(?![^\da-zA-Z]+$).{6,20}$");
                if (res) {
                    return AjaxResult.Success<T>(1000, "密码验证成功");
                }
            }
            return AjaxResult.Error<T>(_errMsg);
        }
    }
}