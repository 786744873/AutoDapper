using XDF.Core.Entity;
using XDF.Data;

namespace XDF.Service
{
    /// <summary>
    /// 
    /// </summary>    
    public partial class ConfigService :BaseService<ConfigEntity>
    {
        private readonly ConfigDao _ConfigDao;
        public ConfigService()
        {
            _ConfigDao = new ConfigDao();
            BaseCrudDao = _ConfigDao;
        }
        private static ConfigService _instance = null;
        private static readonly object Lock = new object();
        public static ConfigService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ConfigService();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
