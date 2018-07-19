using System;
using System.Collections.Generic;
using System.Text;
using XDF.Core.Entity;

namespace XDF.Data
{
    /// <summary>
    /// Dao
    /// </summary>
    public partial class SchoolDao : BaseDao<SchoolEntity>
    {
        public SchoolDao() : base("XDF")
        {
            base._Field = " [sCode],[sName],[sDescription],[sAddress],[sPostCode],[sPhone],[sFax],[sHomePage],[sEmail],[sMemo],[bValid],[sWebServiceUrl],[bCanWebService],[nOverdueMinutes],[nOverdue],[iExt1],[iExt2],[iExt3],[iExt4],[sExt1],[sExt2],[sExt3],[sExt4],[bVoucher],[bCredit],[bSMSon],[DisabledStartDate],[DisabledEndDate],[isUpdateUrl],[sUpdateUrl],[sRefundNoticeEmails],[sNotes],[IsRememberIP],[AlipayMail],[SchoolAbbr],[IsCompatible],[sBackRecUrl],[sFrontRecUrl],[sSignKey],[WechatPayBank],[AliPayBank] ";
            base._Primary = "id";
            base._FieldAt = " @sCode,@sName,@sDescription,@sAddress,@sPostCode,@sPhone,@sFax,@sHomePage,@sEmail,@sMemo,@bValid,@sWebServiceUrl,@bCanWebService,@nOverdueMinutes,@nOverdue,@iExt1,@iExt2,@iExt3,@iExt4,@sExt1,@sExt2,@sExt3,@sExt4,@bVoucher,@bCredit,@bSMSon,@DisabledStartDate,@DisabledEndDate,@isUpdateUrl,@sUpdateUrl,@sRefundNoticeEmails,@sNotes,@IsRememberIP,@AlipayMail,@SchoolAbbr,@IsCompatible,@sBackRecUrl,@sFrontRecUrl,@sSignKey,@WechatPayBank,@AliPayBank ";
            base._Table = " S_School ";
            base._FieldUp = " [sCode]=@sCode,[sName]=@sName,[sDescription]=@sDescription,[sAddress]=@sAddress,[sPostCode]=@sPostCode,[sPhone]=@sPhone,[sFax]=@sFax,[sHomePage]=@sHomePage,[sEmail]=@sEmail,[sMemo]=@sMemo,[bValid]=@bValid,[sWebServiceUrl]=@sWebServiceUrl,[bCanWebService]=@bCanWebService,[nOverdueMinutes]=@nOverdueMinutes,[nOverdue]=@nOverdue,[iExt1]=@iExt1,[iExt2]=@iExt2,[iExt3]=@iExt3,[iExt4]=@iExt4,[sExt1]=@sExt1,[sExt2]=@sExt2,[sExt3]=@sExt3,[sExt4]=@sExt4,[bVoucher]=@bVoucher,[bCredit]=@bCredit,[bSMSon]=@bSMSon,[DisabledStartDate]=@DisabledStartDate,[DisabledEndDate]=@DisabledEndDate,[isUpdateUrl]=@isUpdateUrl,[sUpdateUrl]=@sUpdateUrl,[sRefundNoticeEmails]=@sRefundNoticeEmails,[sNotes]=@sNotes,[IsRememberIP]=@IsRememberIP,[AlipayMail]=@AlipayMail,[SchoolAbbr]=@SchoolAbbr,[IsCompatible]=@IsCompatible,[sBackRecUrl]=@sBackRecUrl,[sFrontRecUrl]=@sFrontRecUrl,[sSignKey]=@sSignKey,[WechatPayBank]=@WechatPayBank,[AliPayBank]=@AliPayBank ";
            base._IsIdentity = false;//主建是否自增，如果不自增插入的时候要赋值
        }
    }
}
