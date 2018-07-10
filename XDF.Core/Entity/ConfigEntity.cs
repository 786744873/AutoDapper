using System;
using System.Collections.Generic;
using System.Text;

namespace XDF.Core.Entity
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public partial class ConfigEntity
    {
        #region 
        /// <summary>
        /// 
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string SName { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string SValue { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string SDescription { set; get; } = "";
        #endregion
    }
}
