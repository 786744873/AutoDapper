using System;
using System.Collections.Generic;
using System.Text;
using XDF.Core.Entity;

namespace XDF.Data
{
    /// <summary>
    /// Dao
    /// </summary>
    public partial class EmployeeDao : BaseDao<EmployeeEntity> 
    {
        public EmployeeDao() : base("XDF")
        {
            base._Field = " NSchoolId,SName,SCode,SNameSpell,NGender,SPhone,SFax,SEmail,SDeptCode,SAddress,SPostCode,SMemo,BValid,NInUsed,SLoginName,SPassword,PwdChangeDate ";
            base._Primary = " id ";
            base._FieldAt = " @NSchoolId,@SName,@SCode,@SNameSpell,@NGender,@SPhone,@SFax,@SEmail,@SDeptCode,@SAddress,@SPostCode,@SMemo,@BValid,@NInUsed,@SLoginName,@SPassword,@PwdChangeDate ";
            base._Table = " S_Employee ";
            base._FieldUp = " NSchoolId=@NSchoolId,SName=@SName,SCode=@SCode,SNameSpell=@SNameSpell,NGender=@NGender,SPhone=@SPhone,SFax=@SFax,SEmail=@SEmail,SDeptCode=@SDeptCode,SAddress=@SAddress,SPostCode=@SPostCode,SMemo=@SMemo,BValid=@BValid,NInUsed=@NInUsed,SLoginName=@SLoginName,SPassword=@SPassword,PwdChangeDate=@PwdChangeDate ";
            base._IsIdentity = true;//主建是否自增，如果不自增插入的时候要赋值
        }
    }
}
