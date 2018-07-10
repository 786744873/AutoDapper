using System;
using XDF.Core.Helper.Ajax;
using XDF.Core.Helper.Ext;
namespace XDF.Core.Validation
{
    public class LengthAttribute : ValidationAttribute
    {
        private readonly string _errMsg;
        private readonly int _max;
        private readonly int _min;
        public LengthAttribute(int min, int max, string errMsg = "该字段长度不对")
        {
            _min = min;
            _max = max;
            _errMsg = errMsg;
        }
        public override AjaxResultModel<T> IsValid<T>(object value)
        {
            if (!value.IsStringEmpty()) {
                var len = value.ToString().Length;
                if (!(len > _max || len < _min)) {
                    return AjaxResult.Success<T>(1000, "验证成功");
                }
            }
            return AjaxResult.Error<T>(_errMsg);
        }
    }
}