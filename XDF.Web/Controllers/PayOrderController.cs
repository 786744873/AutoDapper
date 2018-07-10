using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XDF.Core.Entity;
using XDF.Core.Model;
using XDF.Service;

namespace XDF.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayOrderController : ControllerBase
    {
        [HttpGet]
        public PageListModel<PayOrderEntity> Get([FromQuery]PageListModel<PayOrderEntity> info)
        {
            var res = PayOrderService.Instance.Filter(info);
            return res;
        }
    }
}