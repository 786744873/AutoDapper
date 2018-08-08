using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using XDF.Core.Helper.Ajax;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace System
{
    public static class Extension
    {
        #region Enum 扩展
        public static IDictionary<int, string> GetListItems<TEnum>(this TEnum enums)
        {
            IDictionary<int, string> result = new Dictionary<int, string>();
            foreach (var item in System.Enum.GetValues(typeof(TEnum))) {
                result.Add((int) item, item.ToString());
            }
            return result;
        }
        #endregion
        #region 根据月份返回月末date
        public static void ToDateEnd(string month, out string strBegin, out string strEnd)
        {
            month = month.TrimEnd();
            var day = DateTime.Parse(DateTime.Parse(month).ToString("yyyy-MM-01")).AddMonths(1).AddDays(-1).Day;
            strBegin = $"{month}-01 00:00:00";
            strEnd = $"{month}-{day} 23:59:59";
        }
        #endregion
        #region 组合缓存健
        /// <summary>
        ///     组合缓存健
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetCacheKey0(this string obj, object value)
        {
            return string.Format(obj, value);
        }
        /// <summary>
        ///     组合缓存健
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetCacheKey(this string obj, params object[] value)
        {
            return string.Format(obj, value);
        }
        #endregion
        #region 字符串 中文与Unicode 转换 扩展
        /// <summary>
        ///     中文转unicode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string CnToUnicode(this string obj)
        {
            var outStr = "";
            if(!string.IsNullOrEmpty(obj)) {
                for (var i = 0; i < obj.Length; i++) {
                    //将中文字符转为10进制整数，然后转为16进制unicode字符  
                    outStr += @"/u" + ((int) obj[i]).ToString("x");
                }
            }
            return outStr;
        }
        /// <summary>
        ///     unicode转成中文
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string UnicodeToCn(this string obj)
        {
            var outStr = "";
            if(obj.IsStringEmpty()) {
                return "";
            }
            if(obj.IndexOf(@"/u", StringComparison.Ordinal) < 0) {
                return obj;
            }
            if(!string.IsNullOrEmpty(obj)) {
                var strlist = obj.Replace(@"/", "").Split('u');
                try {
                    for (var i = 1; i < strlist.Length; i++) {
                        //将unicode字符转为10进制整数，然后转为char中文字符  
                        outStr += (char) int.Parse(strlist[i], NumberStyles.HexNumber);
                    }
                }
                catch (FormatException ex) {
                    outStr = ex.Message;
                }
            }
            return outStr;
        }
        #endregion
        #region 字符串 URL HttpUtility 转换 扩展
        public static string UrlDecode(this string obj)
        {
            if(obj.IsStringEmpty()) {
                return "";
            }
            return WebUtility.UrlDecode(obj);
        }
        public static string UrlEnCode(this string obj)
        {
            if(obj.IsStringEmpty()) {
                return "";
            }
            return WebUtility.UrlEncode(obj);
        }
        /// <summary>
        ///     过滤危险字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SafeStr(this string str)
        {
            if(string.IsNullOrEmpty(str)) {
                return "";
            }
            return WebUtility.HtmlEncode(str.Replace("script", "").Replace("select", "").Replace("insert", "").Replace("delete", "").Replace("update", "").Replace("drop", "").Replace("truncate", "").Replace("table", "").Trim());
        }
        #endregion
        #region 字符串截断与追加 扩展
        /// <summary>
        ///     字符串增加前缀
        /// </summary>
        /// <param name="original">原字符串</param>
        /// <param name="prefix">前缀</param>
        /// <param name="isRefEmpty">源字符串为空时是否返回空</param>
        public static string AddPrefix(this string original, string prefix, bool isRefEmpty = true)
        {
            if(original.IsStringEmpty() && isRefEmpty) {
                return "";
            }
            return string.Concat(prefix, original);
        }
        /// <summary>
        ///     字符串增加后缀
        /// </summary>
        /// <param name="original">原字符串</param>
        /// <param name="suffix">后缀</param>
        /// <param name="isRefEmpty">源字符串为空时是否返回空</param>
        /// <returns></returns>
        public static string AddSuffix(this string original, string suffix, bool isRefEmpty = true)
        {
            if(original.IsStringEmpty() && isRefEmpty) {
                return "";
            }
            return string.Concat(original, suffix);
        }
        /// <summary>
        ///     字符串截断
        /// </summary>
        /// <param name="length">截断长度</param>
        public static string Tranct(this string v, int length, string str = "...")
        {
            if(v.IsStringEmpty()) {
                return "";
            }
            if(v.Length <= length) {
                return v;
            }
            return v.Substring(0, length) + str;
        }
        #endregion
        #region 对象验证 扩展
        /// <summary>
        ///     实体属性转成SQL字段 属性上加 NotMappedAttribute 不会转换
        /// </summary>
        /// <returns></returns>
        public static string ModelFiled2String(this Type type)
        {
            var filed = new List<string>();
            foreach (var pro in type.GetProperties()) {
                if(pro.GetCustomAttributes(typeof(NotMappedAttribute), false).FirstOrDefault() is NotMappedAttribute not) {
                    continue;
                }
                if(pro.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault() is ColumnAttribute not2) {
                    filed.Add(not2.Name);
                }
                else {
                    filed.Add(pro.Name);
                }
            }
            return string.Join(",", filed);
        }
        /// <summary>
        ///     将对象属性转换为key-value对
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary(this object o)
        {
            var map = new Dictionary<string, object>();
            try {
                var t = o.GetType();
                var pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var p in pi) {
                    var mi = p.GetGetMethod();
                    if(mi != null && mi.IsPublic) {
                        map.Add(p.Name, mi.Invoke(o, new object[] { }));
                    }
                }
            }
            catch (Exception ex) {
                return map;
            }
            return map;
        }
        public static bool IsMobile(this string obj)
        {
            if(obj.IsStringEmpty()) {
                return false;
            }
            return Regex.IsMatch(obj, @"^[1]+[3,4,5,7,8]+\d{9}$");
        }
        public static bool IsPhone(this string obj)
        {
            return Regex.IsMatch(obj, @"^[0]+\d{7,12}$");
        }
        public static bool IsDateTime(this object v)
        {
            if(v == null || v == DBNull.Value) {
                return false;
            }
            return DateTime.TryParse(v.ToString(), out _);
        }
        public static bool IsStringEmpty(this object v)
        {
            if(v == null || v == DBNull.Value) {
                return true;
            }
            if(string.IsNullOrEmpty(v.ToString())) {
                return true;
            }
            if(v.ToString() == "" || v.ToString() == "\"\"") {
                return true;
            }
            return false;
        }
        public static AjaxResultModel<T1> Validation<T, T1>(this T v) where T : class
        {
            return XDF.Core.Validation.Validation.Check<T, T1>(v);
        }
        #endregion
        #region 其他（坐标转换、URL网址参数） 扩展
        public static string PointToLocStr(this string obj)
        {
            //POINT (113.733329 34.761971)
            return obj.Replace("POINT (", "").Replace(")", "").Replace(" ", ",");
        }
        /// <summary>
        ///     根据网址设置KEY。Value
        /// </summary>
        /// <param name="v"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static string SetQuery(this object v, string key, string value)
        {
            var url = v.ToString();
            var reg1 = new Regex(@"\?", RegexOptions.IgnoreCase);
            if(!reg1.IsMatch(url)) {
                url += "?";
            }
            var reg2 = new Regex(key, RegexOptions.IgnoreCase);
            if(!reg2.IsMatch(url)) {
                url += "&" + key + "=";
            }
            var reg = new Regex(@"(?<=" + key + "=)[a-z,=,0-9,:,\\s,\\.,-]+", RegexOptions.IgnoreCase);
            if(reg.IsMatch(url)) {
                url = reg.Replace(url, value); //替换
            }
            else {
                url += value;
            }
            return url;
        }
        /// <summary>
        ///     获取参数
        /// </summary>
        /// <param name="v"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetQuery(this string url, string key)
        {
            var Value = "";
            var urlRegex = new Regex(@"(?<=" + key + "=)[a-z,=,0-9,:,\\s,\\.,-]+", RegexOptions.IgnoreCase);
            var m = urlRegex.Match(url);
            if(m.Success) {
                Value = m.Value;
            }
            return Value;
        }
        #endregion
        #region 隐藏手机/邮箱 扩展
        /// <summary>
        ///     隐藏手机
        /// </summary>
        public static string HideMobile(this string mobile)
        {
            return mobile.Substring(0, 3) + "*****" + mobile.Substring(8);
        }
        public static string HideName(this string name)
        {
            if(name.IsStringEmpty()) {
                return "";
            }
            var newName = "";
            var id = name.ToCharArray();
            for (var i = 0; i < id.Length; i++) {
                if(i == 0) {
                    newName += "*";
                }
                else {
                    newName += id[i];
                }
            }
            return newName;
        }
        /// <summary>
        ///     保留前几位，后边全部替换成指定字符串如9999替 换成:9***
        /// </summary>
        /// <param name="str"></param>
        /// <param name="res">保留</param>
        /// <param name="rep"></param>
        /// <returns></returns>
        public static string RegxReserve(this string str, int res, string rep = "*")
        {
            var Regex = new Regex(@"(?<=\w{" + res + "})\\w", RegexOptions.IgnoreCase);
            return Regex.Replace(str, rep);
        }
        /// <summary>
        ///     隐藏邮箱
        /// </summary>
        public static string HideEmail(this string email)
        {
            var index = email.LastIndexOf('@');
            if(index == 1) {
                return "*" + email.Substring(index);
            }
            if(index == 2) {
                return email[0] + "*" + email.Substring(index);
            }
            var sb = new StringBuilder();
            sb.Append(email.Substring(0, 2));
            var count = index - 2;
            while (count > 0) {
                sb.Append("*");
                count--;
            }
            sb.Append(email.Substring(index));
            return sb.ToString();
        }
        #endregion
        #region Object 对象转值类型 扩展
        /// <summary>
        ///     对象值转换为字节
        /// </summary>
        public static byte ToByte(this object v, byte defaultVal = 0)
        {
            byte id = 0;
            if(v == null || v == DBNull.Value) {
                return defaultVal;
            }
            try {
                id = Convert.ToByte(v);
            }
            catch {
                byte.TryParse(v.ToString(), out id);
            }
            return id;
        }
        /// <summary>
        ///     对象值转换为短整形
        /// </summary>
        public static short ToShort(this object v, short defaultVal = 0)
        {
            short id = 0;
            if(v == null || v == DBNull.Value) {
                return defaultVal;
            }
            try {
                id = Convert.ToInt16(v);
            }
            catch {
                short.TryParse(v.ToString(), out id);
            }
            return id;
        }
        /// <summary>
        ///     对象值转换为整形 如果为空返回的值
        /// </summary>
        public static int ToInt(this object v, int defaultVal = 0)
        {
            var id = 0;
            if(v == null || v == DBNull.Value) {
                return defaultVal;
            }
            try {
                id = Convert.ToInt32(v);
            }
            catch {
                int.TryParse(v.ToString(), out id);
            }
            return id;
        }
        /// <summary>
        ///     对象值转换为长整形
        /// </summary>
        public static long ToLong(this object v, long defaultVal = 0)
        {
            long id = 0;
            if(v == null || v == DBNull.Value) {
                return defaultVal;
            }
            try {
                id = Convert.ToInt64(v);
            }
            catch {
                long.TryParse(v.ToString(), out id);
            }
            return id;
        }
        /// <summary>
        ///     对象值转换为浮点精度
        /// </summary>
        public static double ToDouble(this object v, double defaultVal = 0.0d)
        {
            var id = 0.0d;
            if(v == null || v == DBNull.Value) {
                return defaultVal;
            }
            try {
                id = Convert.ToDouble(v);
            }
            catch {
                double.TryParse(v.ToString(), out id);
            }
            return id;
        }
        /// <summary>
        ///     对象值转换为浮点精度
        /// </summary>
        public static decimal ToDecimal(this object v, decimal defaultVal = 0.00M)
        {
            var id = 0.00M;
            if(v == null || v == DBNull.Value) {
                return defaultVal;
            }
            try {
                id = Convert.ToDecimal(v);
            }
            catch {
                decimal.TryParse(v.ToString(), out id);
            }
            return Math.Round(id, 2, MidpointRounding.AwayFromZero);
        }
        /// <summary>
        ///     json转成dynamic
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static T ToModel<T>(this string v)
        {
            try {
                return JsonConvert.DeserializeObject<T>(v);
            }
            catch (Exception ex) {
               // AppLogService.Instance.Error(v + ":" + ex.Message);
                return default(T);
            }
        }
        #endregion
        #region 值 加密解密 扩展
        /// <summary>
        ///     值解密处理
        /// </summary>
        public static int DeKey(this object v)
        {
            return v.ToInt(); //PaoTui.Agency.Services.EncryptId.DeKey(v);
        }
        /// <summary>
        ///     值加密处理
        /// </summary>
        public static string EnKey(this object v)
        {
            return v.ToString(); //PaoTui.Agency.Services.EncryptId.EnKey(v);
        }
        #endregion
        #region IQueryable 表达式扩展
        /// <summary>
        ///     IQueryable OrderBy排序扩展
        /// </summary>
        /// <param name="propertyName">排序字段+方式</param>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string propertyName)
        {
            var sort = propertyName.Split(' ');
            return queryable.OrderBy(sort[0], sort[1]);
        }
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string field, string order)
        {
            var param = Expression.Parameter(typeof(T));
            dynamic keySelector = Expression.Lambda(Expression.Property(param, field), param);
            if(order == "desc") {
                return Queryable.OrderByDescending(queryable, keySelector);
            }
            return Queryable.OrderBy(queryable, keySelector);
        }
        #endregion
        #region 字符串 过滤 扩展
        public static string LambdaToSqlFiled(this string v)
        {
            var r = @"(?<=\.)\w+";
            if(v.IsStringEmpty()) {
                return "*";
            }
            var Regex = new Regex(r, RegexOptions.IgnoreCase);
            var matches = Regex.Matches(v);
            var sql = new StringBuilder();
            for (var i = 0; i < matches.Count; i++) {
                if(i > 0) {
                    sql.Append(",");
                }
                sql.Append(matches[i].Value);
            }
            return sql.ToString().Length <= 0 ? "*" : sql.ToString().ToUpper();
        }
        /// <summary>
        ///     /// 转全角的函数(SBC case)
        ///     任意字符串
        ///     全角字符串
        ///     全角空格为12288，半角空格为32
        ///     其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(this string input)
        {
            // 半角转全角：
            var c = input.ToCharArray();
            for (var i = 0; i < c.Length; i++) {
                if(c[i] == 32) {
                    c[i] = (char) 12288;
                    continue;
                }
                if(c[i] < 127) {
                    c[i] = (char) (c[i] + 65248);
                }
            }
            return new string(c);
        }
        public static string EvalSqlString(this string inputString)
        {
            var SqlStr = @"\;|\--|\'|and| or |exec|execute|insert|select|delete|update|alter|create|drop|count|\*|chr|char|asc|mid|substring|master|truncate|declare|xp_cmdshell|restore|backup|net +user|net +localgroup +administrators";
            try {
                if(!string.IsNullOrEmpty(inputString)) {
                    var str_Regex = @"(" + SqlStr + @")";
                    var Regex = new Regex(str_Regex, RegexOptions.Compiled);
                    var matches = Regex.Matches(inputString);
                    for (var i = 0; i < matches.Count; i++) {
                        inputString = inputString.Replace(matches[i].Value, matches[i].Value.ToSBC());
                    }
                }
            }
            catch {
                return "";
            }
            return inputString;
        }
        public static string EvalFilterString(this string inputString)
        {
            //var res = inputString;

            //for (int i = 0; i < inputString.Length; i++)
            //{
            //    if ((inputString[i] & 0xF8) == 0xF0)
            //    {
            //        for (int j = 0; j < 4; j++)
            //        {
            //            res[i + j] = 0x30;
            //        }
            //        i += 3;
            //    }
            //}
            var SqlStr = @"[^\\]";
           // var SqlStr = @"[.~!@#$%\^\+\*&\\\/\?\|:\.{}()';=]";
            try
            {
                if (!string.IsNullOrEmpty(inputString))
                {
                    var strRegex = @"(" + SqlStr + @")";
                    var regex = new Regex(strRegex, RegexOptions.Compiled);
                    var matches = regex.Matches(inputString);
                    for (var i = 0; i < matches.Count; i++)
                    {
                        inputString = inputString.Replace(matches[i].Value, matches[i].Value.ToSBC());
                    }
                }
            }
            catch
            {
                return "";
            }
            return inputString;
        }
        //[.~!@#$%\^\+\*&\\\/\?\|:\.{}()';="]
        public static string EvalEmjoyString(this string inputString)
        {
            var SqlStr = @"(?:[\uD83C\uDF00-\uD83D\uDDFF]|[\uD83E\uDD00-\uD83E\uDDFF]|[\uD83D\uDE00-\uD83D\uDE4F]|[\uD83D\uDE80-\uD83D\uDEFF]|[\u2600-\u26FF]\uFE0F?|[\u2700-\u27BF]\uFE0F?|\u24C2\uFE0F?|[\uD83C\uDDE6-\uD83C\uDDFF]{1,2}|[\uD83C\uDD70\uD83C\uDD71\uD83C\uDD7E\uD83C\uDD7F\uD83C\uDD8E\uD83C\uDD91-\uD83C\uDD9A]\uFE0F?|[\u0023\u002A\u0030-\u0039]\uFE0F?\u20E3|[\u2194-\u2199\u21A9-\u21AA]\uFE0F?|[\u2B05-\u2B07\u2B1B\u2B1C\u2B50\u2B55]\uFE0F?|[\u2934\u2935]\uFE0F?|[\u3030\u303D]\uFE0F?|[\u3297\u3299]\uFE0F?|[\uD83C\uDE01\uD83C\uDE02\uD83C\uDE1A\uD83C\uDE2F\uD83C\uDE32-\uD83C\uDE3A\uD83C\uDE50\uD83C\uDE51]\uFE0F?|[\u203C\u2049]\uFE0F?|[\u25AA\u25AB\u25B6\u25C0\u25FB-\u25FE]\uFE0F?|[\u00A9\u00AE]\uFE0F?|[\u2122\u2139]\uFE0F?|\uD83C\uDC04\uFE0F?|\uD83C\uDCCF\uFE0F?|[\u231A\u231B\u2328\u23CF\u23E9-\u23F3\u23F8-\u23FA]\uFE0F?)";
            try {
                if(!string.IsNullOrEmpty(inputString)) {
                    var strRegex = @"(" + SqlStr + @")";
                    var regex = new Regex(strRegex, RegexOptions.Compiled);
                    var matches = regex.Matches(inputString);
                    for (var i = 0; i < matches.Count; i++) {
                        inputString = inputString.Replace(matches[i].Value, matches[i].Value.ToSBC());
                    }
                }
            }
            catch {
                return "";
            }
            return inputString;
        }
        #endregion
        #region 字符串转 Object 对象 扩展
        public static object ToObject(this string whereVal, Type valType)
        {
            object whereValue = null;
            if(string.IsNullOrEmpty(whereVal)) {
                return whereValue;
            }
            if(valType == typeof(string) || valType == typeof(string)) {
                whereValue = Convert.ToString(whereVal).Replace("\r\n", "").Trim();
            }
            else if(valType == typeof(byte) || valType == typeof(byte?)) {
                byte outval = 0;
                if(byte.TryParse(whereVal, out outval)) {
                    whereValue = outval;
                }
                else {
                    whereValue = null;
                }
            }
            else if(valType == typeof(short) || valType == typeof(short?) || valType == typeof(short) || valType == typeof(short?)) {
                short outval = 0;
                if(short.TryParse(whereVal, out outval)) {
                    whereValue = outval;
                }
                else {
                    whereValue = null;
                }
            }
            else if(valType == typeof(ushort) || valType == typeof(ushort?)) {
                ushort outval = 0;
                if(ushort.TryParse(whereVal, out outval)) {
                    whereValue = outval;
                }
                else {
                    whereValue = null;
                }
            }
            else if(valType == typeof(int) || valType == typeof(int?) || valType == typeof(int) || valType == typeof(int?)) {
                var outval = 0;
                if(int.TryParse(whereVal, out outval)) {
                    whereValue = outval;
                }
                else {
                    whereValue = null;
                }
            }
            else if(valType == typeof(uint) || valType == typeof(uint?) || valType == typeof(uint) || valType == typeof(uint?)) {
                uint outval = 0;
                if(uint.TryParse(whereVal, out outval)) {
                    whereValue = outval;
                }
                else {
                    whereValue = null;
                }
            }
            else if(valType == typeof(long) || valType == typeof(long?) || valType == typeof(long) || valType == typeof(long?)) {
                long outval = 0;
                if(long.TryParse(whereVal, out outval)) {
                    whereValue = outval;
                }
                else {
                    whereValue = null;
                }
            }
            else if(valType == typeof(ulong) || valType == typeof(ulong?) || valType == typeof(ulong) || valType == typeof(ulong?)) {
                long outval = 0;
                if(long.TryParse(whereVal, out outval)) {
                    whereValue = outval;
                }
                else {
                    whereValue = null;
                }
            }
            else if(valType == typeof(float) || valType == typeof(float?) || valType == typeof(float) || valType == typeof(float?)) {
                float outval = 0;
                if(float.TryParse(whereVal, out outval)) {
                    whereValue = outval;
                }
                else {
                    whereValue = null;
                }
            }
            else if(valType == typeof(double) || valType == typeof(double?)) {
                double outval = 0;
                if(double.TryParse(whereVal, out outval)) {
                    whereValue = outval;
                }
                else {
                    whereValue = null;
                }
            }
            else if(valType == typeof(decimal) || valType == typeof(decimal?)) {
                decimal outval = 0;
                if(decimal.TryParse(whereVal, out outval)) {
                    whereValue = outval;
                }
                else {
                    whereValue = null;
                }
            }
            else if(valType == typeof(DateTime) || valType == typeof(DateTime?)) {
                var outval = DateTime.Parse("1900-01-01");
                if(DateTime.TryParse(whereVal, out outval)) {
                    whereValue = outval;
                }
                else {
                    whereValue = null;
                }
            }
            else if(valType == typeof(TimeSpan) || valType == typeof(TimeSpan?)) {
                var outval = TimeSpan.Parse("00:00:00");
                if(TimeSpan.TryParse(whereVal, out outval)) {
                    whereValue = outval;
                }
                else {
                    whereValue = null;
                }
            }
            else if(valType == typeof(bool) || valType == typeof(bool?)) {
                whereValue = whereVal == "1" || whereVal.ToLower() == "true";
            }
            return whereValue;
        }
        /// <summary>
        ///     转json
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string ToJson(this object v, string format = "yyyy-MM-dd HH:mm:ss")
        {
            var timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = format;
            var Settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };
            return JsonConvert.SerializeObject(v, Formatting.None, timeConverter);
        }
        #endregion

        #region DeserializeUtf8
        public static string DeserializeUtf8(this byte[] stream)
        {
            return stream == null ? (string)null : Encoding.UTF8.GetString(stream);
        }
        public static byte[] SerializeUtf8(this string str)
        {
            return str == null ? (byte[])null : Encoding.UTF8.GetBytes(str);
        } 
        #endregion
    }
}