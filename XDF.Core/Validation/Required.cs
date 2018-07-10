using System;
using XDF.Core.Helper.Ajax;
using XDF.Core.Helper.Ext;
namespace XDF.Core.Validation
{
    public class RequiredAttribute : ValidationAttribute
    {
        private readonly string _errMsg;
        public RequiredAttribute(string errMsg = "该字段不能为空")
        {
            _errMsg = errMsg;
        }
        public override AjaxResultModel<T> IsValid<T>(object value)
        {
            if (!value.IsStringEmpty()) {
                return AjaxResult.Success<T>(1000, "验证成功");
            }
            return AjaxResult.Error<T>(Filed.IsStringEmpty() ? _errMsg : Filed + "不能为空");
        }
    }
}