using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Newtonsoft.Json;
namespace XDF.Core.Model
{
    [Serializable]
    public class PageListMongoModel<T>
    {
        public PageListMongoModel()
        {
            List = new List<T>();
            PageIndex = 1;
            PageSize = 20;
            Count = 0;
            Where = s => true;
            OrderBy = s => true;
        }
        /// <summary>
        ///     分页当前数据
        /// </summary>
        public List<T> List { get; set; }
        /// <summary>
        ///     当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        ///     每页行数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        ///     总行数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        ///     总页数
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        ///     如果为true取记录条数。默认为true
        /// </summary>
        [JsonIgnore]
        public bool IsGetCount { get; set; }
        [JsonIgnore]
        public Expression<Func<T, bool>> OrderBy { get; }
        [JsonIgnore]
        public Expression<Func<T, bool>> Where { get; }
    }
}