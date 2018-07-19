
using XDF.Core.Entity;

namespace XDF.Data
{
    /// <summary>
    /// UserDao
    /// </summary>
    public partial class UserDao : BaseDao<UserEntity>
    {
        public UserDao() : base("YSKJ")
        {
            base._Field = " RealName,AccoutState,LoginName,PlainPassword,PassWord,LoginTimes,IsLogin,Department,PostJob,Mobile,Email,CityCode,AddTime,LastLoginTime,LastLoginIp ";
            base._Primary = "Id";
            base._FieldAt = " @RealName,@AccoutState,@LoginName,@PlainPassword,@PassWord,@LoginTimes,@IsLogin,@Department,@PostJob,@Mobile,@Email,@CityCode,@AddTime,@LastLoginTime,@LastLoginIp ";
            base._Table = " User ";
            base._FieldUp = " RealName=@RealName,AccoutState=@AccoutState,LoginName=@LoginName,PlainPassword=@PlainPassword,PassWord=@PassWord,LoginTimes=@LoginTimes,IsLogin=@IsLogin,Department=@Department,PostJob=@PostJob,Mobile=@Mobile,Email=@Email,CityCode=@CityCode,AddTime=@AddTime,LastLoginTime=@LastLoginTime,LastLoginIp=@LastLoginIp ";
            base._IsIdentity = true;//主建是否自增，如果不自增插入的时候要赋值
        }
    }
}

