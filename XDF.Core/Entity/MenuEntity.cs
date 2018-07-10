using System;
namespace XDF.Core.Entity
{
    /// <summary>
    /// Model
    /// </summary>
    [Serializable]
    public partial class MenuEntity
    {

        /// <summary>
        /// 
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? Index { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? FuncID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string urlAddr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string imgAddr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? Level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? ParentSeq { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? IsTheSamePage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? Creator { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? Modifier { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ModifyTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? CompanyID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? IsDelete { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? SystemID { get; set; }
    }
}

