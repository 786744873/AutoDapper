using System;
using System.Collections.Generic;
using System.Text;
using XDF.Core.Entity;
using XDF.Data;

namespace XDF.Service
{
    /// <summary>
    /// FunctionService
    /// </summary>
    public partial class FunctionService : BaseService<FunctionEntity>
    {
        public readonly FunctionDao _Functiondao = null;
        public FunctionService()
        {
            this._Functiondao = new FunctionDao();
            base.BaseCrudDao = this._Functiondao;
        }
        private static FunctionService _Instance = null;
        private static readonly object _lock = new object();
        public static FunctionService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_lock)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new FunctionService();
                        }
                    }
                }
                return _Instance;
            }
        }

    }
}
