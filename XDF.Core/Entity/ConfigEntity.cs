using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using MySqlX.XDevAPI.Relational;

namespace XDF.Core.Entity
{
    /// <summary>
    /// 
    /// </summary>
    [Table("BS_Config")]
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
