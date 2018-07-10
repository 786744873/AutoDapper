using System;
using XDF.Core.Helper.Ajax;
using XDF.Core.Helper.Ext;
namespace XDF.Core.Validation
{
    public class RangeAttribute : ValidationAttribute
    {
        private readonly string _errMsg;
        private readonly int _max;
        private readonly int _min;
        public RangeAttribute(int min, int max, string errMsg = "该字段不在此区间内")
        {
            _min = min;
            _max = max;
            _errMsg = errMsg;
        }
        public override AjaxResultModel<T> IsValid<T>(object value)
        {
            var val = value.ToInt();
            if (val >= _min && val <= _max) {
                return AjaxResult.Success<T>(1000, "区间验证成功");
            }
            return AjaxResult.Error<T>(_errMsg);
        }
    }
}