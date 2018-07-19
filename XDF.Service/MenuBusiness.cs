

using XDF.Core.Entity;
using XDF.Data;

namespace XDF.Service
{
    /// <summary>
    /// service
    /// </summary>
    public partial class MenuService : BaseService<MenuEntity>
    {
        public readonly MenuDao _Menudao = null;
        public MenuService()
        {
            this._Menudao = new MenuDao();
            base.BaseCrudDao = this._Menudao;
        }
        private static MenuService _Instance = null;
        private static readonly object _lock = new object();
        public static MenuService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_lock)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new MenuService();
                        }
                    }
                }
                return _Instance;
            }
        }

    }
}