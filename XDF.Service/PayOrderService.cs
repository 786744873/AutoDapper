using System;
using System.Collections.Generic;
using System.Text;
using XDF.Core.Entity;
using XDF.Data;

namespace XDF.Service
{
    public partial class PayOrderService : BaseService<PayOrderEntity>
    {
        private readonly PayOrderDao _PayOrderDao;
        public PayOrderService()
        {
            _PayOrderDao = new PayOrderDao();
            BaseCrudDao = _PayOrderDao;
        }
        private static PayOrderService _instance = null;
        private static readonly object Lock = new object();
        public static PayOrderService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new PayOrderService();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
