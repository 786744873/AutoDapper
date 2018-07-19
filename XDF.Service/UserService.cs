
using XDF.Core.Entity;
using XDF.Data;

namespace XDF.Service
{
    /// <summary>
    /// UserService
    /// </summary>
    public partial class UserService : BaseService<UserEntity>
    {
        private readonly UserDao _Userdao = null;
        public UserService()
        {
            this._Userdao = new UserDao();
            base.BaseCrudDao = this._Userdao;
        }
        private static UserService _Instance = null;
        private static readonly object _lock = new object();
        public static UserService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_lock)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new UserService();
                        }
                    }
                }
                return _Instance;
            }
        }

    }
}