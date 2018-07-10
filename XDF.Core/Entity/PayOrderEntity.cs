using System;
using System.Collections.Generic;
using System.Text;

namespace XDF.Core.Entity
{
    public class PayOrderEntity
    {
        #region 
        /// <summary>
        /// 
        /// </summary>
        public int Id { set; get; } = 0;
        /// <summary>
        /// 支付号（内部编号，与订单对应）
        /// </summary>
        public int PayOrderId { set; get; }
        /// <summary>
        /// 商家ID 即 网报的学校ID 如果 此支付是多个学校合并支付那么ID填写为0作为标志字段
        /// </summary>
        public int BusinessId { set; get; } = 0;
        /// <summary>
        /// 支付单 与订单关系字段
        /// </summary>
        public string PayOrderid_OrderCode { set; get; } = "";
        /// <summary>
        /// 支付单号（给接口银行的）
        /// </summary>
        public string PayOrderCode { set; get; } = "";
        /// <summary>
        /// 是否是合并支付
        /// </summary>
        public bool? IsUnite { set; get; } = false;
        /// <summary>
        /// 支付状态
        /// </summary>
        public int? PayState { set; get; } = 0;
        /// <summary>
        /// 失败类型
        /// </summary>
        public int? PayFailType { set; get; } = 0;
        /// <summary>
        /// 应支付金额
        /// </summary>
        public string PayMoney { set; get; } = "";
        /// <summary>
        /// 已支付金额
        /// </summary>
        public string PaidupMoney { set; get; } = "";
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayTime { set; get; } = new DateTime(1970, 01, 01);
        /// <summary>
        /// 支付接口Id
        /// </summary>
        public int? PayPlatform { set; get; } = 0;
        /// <summary>
        /// 支付接口名称
        /// </summary>
        public string PayPlatformName { set; get; } = "";
        /// <summary>
        /// 直连银行ID
        /// </summary>
        public int? BankID { set; get; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public string BankReturnCode { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public bool IsNotify { set; get; } = false;
        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime? CreateTime { set; get; } = new DateTime(1970, 01, 01);
        /// <summary>
        /// 备注字段
        /// </summary>
        public string Memo { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public int? NExt1 { set; get; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public int? NExt2 { set; get; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public string SExt1 { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string SExt2 { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public bool? SubAccounts { set; get; } = false;
        /// <summary>
        /// 代理人（代理商）编号
        /// </summary>
        public string OperatorId { set; get; } = "";
        /// <summary>
        /// 第三方单号
        /// </summary>
        public string ThirdNo { set; get; } = "";
        /// <summary>
        /// 支付平台类型
        /// </summary>
        public string PlatformType { set; get; } = "";
        /// <summary>
        /// 客户订单号
        /// </summary>
        public string CustomerNo { set; get; } = "";
        #endregion
    }
}
