using System.Text.RegularExpressions;
using XDF.Core.Helper.Ajax;
namespace XDF.Core.Validation
{
    public class PriceAttribute : ValidationAttribute
    {
        private readonly string _errMsg;
        public PriceAttribute(string errMsg = "价格不正确")
        {
            _errMsg = errMsg;
        }
        public override AjaxResultModel<T> IsValid<T>(object value)
        {
            if (value != null) {
                var res = Regex.IsMatch(value.ToString(), @"^(0|[1-9][0-9]{0,9})(\.[0-9]{1,2})?$");
                if (res) {
                    return AjaxResult.Success<T>(1000, "验证成功");
                }
            }
            return AjaxResult.Error<T>(_errMsg);
        }
    }
}