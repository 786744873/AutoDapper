using System;
using System.Collections.Generic;
using System.Text;

namespace XDF.Core.Entity
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public partial class SchoolEntity
    {
        #region 
        /// <summary>
        /// 
        /// </summary>
        public int Id { set; get; }
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
        public string SDescription { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string SAddress { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string SPostCode { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string SPhone { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string SFax { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string SHomePage { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string SEmail { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string SMemo { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public bool BValid { set; get; } = false;
        /// <summary>
        /// 网上报名web服务地址
        /// </summary>
        public string SWebServiceUrl { set; get; } = "";
        /// <summary>
        /// 是否可以网上报名（可用于临时关闭）
        /// </summary>
        public bool? BCanWebService { set; get; } = true;
        /// <summary>
        /// 
        /// </summary>
        public int NOverdueMinutes { set; get; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public int NOverdue { set; get; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public int? IExt1 { set; get; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public int? IExt2 { set; get; } = 0;
        /// <summary>
        /// 是否开启选座位功能
        /// </summary>
        public int? IExt3 { set; get; } = 0;
        /// <summary>
        /// 是否开启推荐优惠，为1则是开启
        /// </summary>
        public int? IExt4 { set; get; } = 0;
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
        public string SExt3 { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string SExt4 { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public bool BVoucher { set; get; } = false;
        /// <summary>
        /// 是否支持信用卡支付
        /// </summary>
        public bool BCredit { set; get; } = false;
        /// <summary>
        /// 短信发送开关
        /// </summary>
        public bool BSMSon { set; get; } = false;
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DisabledStartDate { set; get; } = new DateTime(1970, 01, 01);
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DisabledEndDate { set; get; } = new DateTime(1970, 01, 01);
        /// <summary>
        /// 
        /// </summary>
        public int? IsUpdateUrl { set; get; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public string SUpdateUrl { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string SRefundNoticeEmails { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string SNotes { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public bool? IsRememberIP { set; get; } = false;
        /// <summary>
        /// 
        /// </summary>
        public string AlipayMail { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string SchoolAbbr { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public bool? IsCompatible { set; get; } = false;
        /// <summary>
        /// 商户接受新东方支付结果的后端地址
        /// </summary>
        public string SBackRecUrl { set; get; } = "";
        /// <summary>
        /// 商户接受新东方支付结果的前端地址
        /// </summary>
        public string SFrontRecUrl { set; get; } = "";
        /// <summary>
        /// 新东方支付传递时签名的密钥
        /// </summary>
        public string SSignKey { set; get; } = "";
        /// <summary>
        /// 
        /// </summary>
        public int WechatPayBank { set; get; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public int AliPayBank { set; get; } = 0;
        #endregion
    }
}
