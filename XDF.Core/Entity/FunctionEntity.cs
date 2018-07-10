using System;
using System.Collections.Generic;
using System.Text;

namespace XDF.Core.Entity
{
    /// <summary>
    /// FunctionModel
    /// </summary>
    [Serializable]
    public partial class FunctionEntity
    {

        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 父级ID
        /// </summary>
        public int PId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Des { get; set; }
        /// <summary>
        /// 请求路径
        /// </summary>
        public string ApiPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UrlPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ComponentPath { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public byte Level { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否是导航
        /// </summary>
        public byte IsNavigation { get; set; }
        /// <summary>
        /// 是否是按钮
        /// </summary>
        public byte IsButton { get; set; }
        /// <summary>
        /// 是否是快捷操作
        /// </summary>
        public byte IsShortcut { get; set; }
    }
}
