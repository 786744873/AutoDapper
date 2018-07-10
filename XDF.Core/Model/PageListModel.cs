using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace XDF.Core.Model
{
    [Serializable]
    public class PageListModel<T>
    {
        public PageListModel()
        {
            List = new List<T>();
            IsGetCount = true;
            PageIndex = 1;
            PageSize = 20;
            WhereVal = "";
            WhereVal = "";
            Limit = 0;
            IsPage = true;
        }
        /// <summary>
        ///     分页当前数据
        /// </summary>
        public List<T> List { get; set; }
        #region 自定义查询相关
        /// <summary>
        ///     筛选字段
        /// </summary>
        public string WhereCol { get; set; }
        /// <summary>
        ///     筛选条件 0:等于 1:包含 2:开始于 3:结束于 4:不包含 5:大于 6:大于等于 7:小于 8:小于等于 9:介于
        /// </summary>
        public string WhereCon { get; set; }
        /// <summary>
        ///     值
        /// </summary>
        public string WhereVal { get; set; }
        /// <summary>
        ///     值1
        /// </summary>
        public string WhereVal1 { get; set; }
        /// <summary>
        ///     过滤条件
        /// </summary>
        [JsonIgnore]
        public string WhereStr { get; set; }
        #endregion
        #region 自定义分组+排序
        /// <summary>
        ///     排序方式 asc正序 Desc倒序
        /// </summary>
        [JsonIgnore]
        public string Order { get; set; }
        /// <summary>
        ///     排序列名
        /// </summary>
        [JsonIgnore]
        public string Sort { get; set; }
        /// <summary>
        ///     自定义排序 = Sort + Order
        /// </summary>
        [JsonIgnore]
        public string OrderBy { get; set; }
        /// <summary>
        ///     分组列表
        /// </summary>
        [JsonIgnore]
        public string GroupBy { get; set; }
        #endregion
        #region 自定义分页
        /// <summary>
        ///     总页数
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        ///     如果为true取记录条数。默认为true
        /// </summary>
        [JsonIgnore]
        public bool IsGetCount { get; set; }
        /// <summary>
        ///     总行数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        ///     是否分页
        /// </summary>
        [JsonIgnore]
        public bool IsPage { get; set; }
        /// <summary>
        ///     当前页码
        /// </summary>
        public int PageIndex { get; set; }
     
        /// <summary>
        ///     每页行数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        ///     每页行数（=PageSize）
        /// </summary>
        [JsonIgnore]
        public int Limit { get; set; }
        /// <summary>
        ///     分页过滤行数= （页码-1*每页行数）
        /// </summary>
        [JsonIgnore]
        public int Offset { get; set; }
        #endregion
        #region 查询自定义表、字段
        /// <summary>
        ///     自定义表名（支持多表联查）
        /// </summary>
        [JsonIgnore]
        public string Table { get; set; }
        /// <summary>
        ///     表主建（多表联查以主表主键为主）
        /// </summary>
        [JsonIgnore]
        public string Primary { get; set; }
        /// <summary>
        ///     查询字段多个以 英文逗号分割
        /// </summary>
        [JsonIgnore]
        public string Field { get; set; } 
        #endregion
    }
}