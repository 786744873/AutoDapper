using XDF.Core.Helper.Ajax;
namespace XDF.Core.Validation
{
    public class Validation
    {
        public static AjaxResultModel<T1> Check<T, T1>(T model) where T : class
        {
            var type = typeof(T);
            var check = AjaxResult.Success(1000, "验证成功", default(T1));
            foreach (var pro in type.GetProperties()) {
                var validation = pro.GetCustomAttributes(typeof(ValidationAttribute), false);
                if (validation != null && validation.Length > 0) {
                    foreach (var m in validation) {
                        if (m is ValidationAttribute vald) {
                            var val = pro.GetValue(model);
                            check = vald.IsValid<T1>(val);
                            if (!check.IsSuccess) {
                                return check;
                            }
                        }
                    }
                }
            }
            return check;
        }
    }
}