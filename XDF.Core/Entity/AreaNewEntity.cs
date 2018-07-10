using System;
using System.Collections.Generic;
using System.Text;

namespace XDF.Core.Entity
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public partial class AreaNewEntity
    {
        #region 
        /// <summary>
        /// 
        /// </summary>
        public int ID { set; get; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public string SCode { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string SName { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string SFatherCode { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public bool? BAuditing { set; get; } = false;
        #endregion
    }
}
