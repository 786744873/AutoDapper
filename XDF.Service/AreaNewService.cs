using XDF.Core.Entity;
using XDF.Data;

namespace XDF.Service
{
    /// <summary>
    /// 
    /// </summary>    
    public partial class AreaNewService : BaseService<AreaNewEntity>
    {
        private readonly AreaNewDao _AreaNewDao;
        public AreaNewService()
        {
            _AreaNewDao = new AreaNewDao();
            BaseCrudDao = _AreaNewDao;
        }
        private static AreaNewService _instance = null;
        private static readonly object Lock = new object();
        public static AreaNewService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new AreaNewService();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
