using System.Text.RegularExpressions;
using XDF.Core.Helper.Ajax;
namespace XDF.Core.Validation
{
    /// <summary>
    ///    url
    /// </summary>
    public class UrlAttribute : ValidationAttribute
    {
        private readonly string _errMsg;
        public UrlAttribute(string errMsg = "Url不正确")
        {
            _errMsg = errMsg;
        }
        public override AjaxResultModel<T> IsValid<T>(object value)
        {
            if (value != null) {
                var res = Regex.IsMatch(value.ToString(), @"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?");
                if (res) {
                    return AjaxResult.Success<T>(1000, "Url验证成功");
                }
            }
            return AjaxResult.Error<T>(_errMsg);
        }
    }
}