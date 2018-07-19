using System;
using System.Collections.Generic;
using System.Text;
using XDF.Core.Entity;

namespace XDF.Data
{
    public partial class PayOrderDao : BaseDao<PayOrderEntity>
    {
        public PayOrderDao() : base("XDF")
        {
            base._Field = " [Id],[BusinessId],[PayOrderid_OrderCode],[PayOrderCode],[IsUnite],[PayState],[PayFailType],[PayMoney],[PaidupMoney],[PayTime],[PayPlatform],[PayPlatformName],[BankID],[BankReturnCode],[IsNotify],[CreateTime],[Memo],[nExt1],[nExt2],[sExt1],[sExt2],[SubAccounts],[OperatorId],[ThirdNo],[PlatformType],[CustomerNo] ";
            base._Primary = "PayOrderId";
            base._FieldAt = " @Id,@BusinessId,@PayOrderid_OrderCode,@PayOrderCode,@IsUnite,@PayState,@PayFailType,@PayMoney,@PaidupMoney,@PayTime,@PayPlatform,@PayPlatformName,@BankID,@BankReturnCode,@IsNotify,@CreateTime,@Memo,@nExt1,@nExt2,@sExt1,@sExt2,@SubAccounts,@OperatorId,@ThirdNo,@PlatformType,@CustomerNo ";
            base._Table = " N3_PayOrder ";
            base._FieldUp = " [Id]=@Id,[BusinessId]=@BusinessId,[PayOrderid_OrderCode]=@PayOrderid_OrderCode,[PayOrderCode]=@PayOrderCode,[IsUnite]=@IsUnite,[PayState]=@PayState,[PayFailType]=@PayFailType,[PayMoney]=@PayMoney,[PaidupMoney]=@PaidupMoney,[PayTime]=@PayTime,[PayPlatform]=@PayPlatform,[PayPlatformName]=@PayPlatformName,[BankID]=@BankID,[BankReturnCode]=@BankReturnCode,[IsNotify]=@IsNotify,[CreateTime]=@CreateTime,[Memo]=@Memo,[nExt1]=@nExt1,[nExt2]=@nExt2,[sExt1]=@sExt1,[sExt2]=@sExt2,[SubAccounts]=@SubAccounts,[OperatorId]=@OperatorId,[ThirdNo]=@ThirdNo,[PlatformType]=@PlatformType,[CustomerNo]=@CustomerNo ";
            base._IsIdentity = false;//主建是否自增，如果不自增插入的时候要赋值
        }
    }
}
