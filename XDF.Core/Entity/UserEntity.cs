using System;
namespace XDF.Core.Entity
{
    /// <summary>
    /// UserModel
    /// </summary>
    [Serializable]
    public partial class UserEntity
    {

        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 账户状态 0未审核 1审核通过 -2未审核通过 -1冻结
        /// </summary>
        public byte AccoutState { get; set; } = 0;
        /// <summary>
        /// 登录账号
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 密码盐
        /// </summary>
        public string PlainPassword { get; set; } = "";
        /// <summary>
        /// 加密密码
        /// </summary>
        public string PassWord { get; set; } = "";

        /// <summary>
        /// 成功登录次数
        /// </summary>
        public int LoginTimes { get; set; } = 0;

        /// <summary>
        /// 是否允许登录
        /// </summary>
        public byte IsLogin { get; set; } = 1;
        /// <summary>
        /// 部门
        /// </summary>
        public string Department { get; set; } = "";
        /// <summary>
        /// 职务
        /// </summary>
        public string PostJob { get; set; } = "";
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string CityCode { get; set; } = "";

        /// <summary>
        /// 新增注册时间
        /// </summary>
        public DateTime AddTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后登录IP
        /// </summary>
        public string LastLoginIp { get; set; } = "0.0.0.0";
    }
}

