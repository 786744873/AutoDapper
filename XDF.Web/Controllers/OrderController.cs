using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace XDF.Web.Controllers
{
    /// <summary>
    /// 订单详情
    /// </summary>
    public class OrderDetail
    {
        /// <summary>
        ///  订单id
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
    }
    /// <summary>
    /// 订单接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="orderCode">订单编号</param>
        /// <returns></returns>
        [HttpGet]
        public OrderDetail Get(string orderCode)
        {
            return new OrderDetail() { OrderId = "1213arweqrwew", UserId = 100 };
        }
    }
}