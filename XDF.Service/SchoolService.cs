using System;
using System.Collections.Generic;
using System.Text;
using XDF.Core.Entity;
using XDF.Data;

namespace XDF.Service
{
    public partial class SchoolService :BaseService<SchoolEntity>
    {
        private readonly SchoolDao _SchoolDao;
        public SchoolService()
        {
            _SchoolDao = new SchoolDao();
            BaseCrudDao = _SchoolDao;
        }
        private static SchoolService _instance = null;
        private static readonly object Lock = new object();
        public static SchoolService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new SchoolService();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
