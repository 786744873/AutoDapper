using System;
using System.Collections.Generic;
using System.Text;
using XDF.Core.Entity;
using XDF.Data;

namespace XDF.Service
{
    public class EmployeeService:BaseService<EmployeeEntity>
    {
        public readonly EmployeeDao _EmployeeDao = null;
        public EmployeeService()
        {
            this._EmployeeDao = new EmployeeDao();
            base.BaseCrudDao = this._EmployeeDao;
        }
        private static EmployeeService _Instance = null;
        private static readonly object _lock = new object();
        public static EmployeeService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_lock)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new EmployeeService();
                        }
                    }
                }
                return _Instance;
            }
        }
    }
}
