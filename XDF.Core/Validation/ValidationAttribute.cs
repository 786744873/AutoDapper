using System;
using XDF.Core.Helper.Ajax;
namespace XDF.Core.Validation
{
    public class ValidationAttribute : Attribute
    {
        public virtual string Filed { get; set; }
        public virtual AjaxResultModel<T> IsValid<T>(object value)
        {
            return AjaxResult.Success(1000, "", default(T));
        }
    }
}