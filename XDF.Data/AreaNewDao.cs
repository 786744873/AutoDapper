using System;
using System.Collections.Generic;
using System.Text;
using XDF.Core.Entity;
   
namespace XDF.Data
{
   public partial class AreaNewDao : BaseDao<AreaNewEntity>
    {
        public AreaNewDao() : base("XDF")
        {
            base._Field = " SCode,SName,SFatherCode,BAuditing ";
            base._Primary = " ID ";
            base._FieldAt = " @SCode,@SName,@SFatherCode,@BAuditing ";
            base._Table = " [BS_AreaNew] ";
            base._FieldUp = " SFatherCode=@SFatherCode,SName=@SName,SCode=@SCode,BAuditing=@BAuditing ";
            base._IsIdentity = false;//主建是否自增，如果不自增插入的时候要赋值
        }
    }
}
