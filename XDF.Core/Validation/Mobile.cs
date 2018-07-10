using System;
using System.Text.RegularExpressions;
using XDF.Core.Helper.Ajax;
using XDF.Core.Helper.Ext;
namespace XDF.Core.Validation
{
    public class MobileAttribute : ValidationAttribute
    {
        private readonly string _errMsg;
        public MobileAttribute(string errMsg = "手机号格式不正确")
        {
            _errMsg = errMsg;
        }
        public override AjaxResultModel<T> IsValid<T>(object value)
        {
            if (value != null) {
                if(value.IsStringEmpty()) {
                    return AjaxResult.Error<T>(Filed.IsStringEmpty() ? _errMsg : Filed + "不能为空");
                }
                var res = Regex.IsMatch(value.ToString(), @"^[1][3,4,5,6,7,8,9]\d{9}$");
                if (res) {
                    return AjaxResult.Success<T>(1000, "验证成功");
                }
            }
            return AjaxResult.Error<T>(Filed.IsStringEmpty() ? _errMsg : Filed + "不正确");
        }
    }
}