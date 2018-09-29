using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XDF.Core.Entity;
using XDF.Core.Helper.Ajax;
using XDF.DapperLambda;

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
        public  string OrderCode { get; set; }
    }

    public class OrderController: BaseController
    {
        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="orderCode">订单编号</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<OrderDetail> Get([FromBody]string orderCode)
        {   
            return new OrderDetail() { OrderCode=orderCode, OrderId = "1213arweqrwew", UserId = 100 };
        }
        [HttpPost]
        [Route("GetOrderDetail")]
        public string GetOrderDetail(OrderDetail orderdetail)
        {
            return "23423";
        }
        [HttpGet]
        [Route("AddConfig")]
        public AjaxResultModel<List<ConfigEntity>> AddConfig()
        {
            var con = new SqlConnection("user id=db_whbmowner;data source=10.202.202.245;password=levitra5gt#;persist security info=True;initial catalog =BJ20140915;Connect Timeout=300");
            var res = con.QuerySet<ConfigEntity>().Where(m=>m.Id>=1).ToList();
            return AjaxResult.Success(res);
        }
        [HttpGet]
        [Route("Index")]
        public async Task<AjaxResultModel<string>> Index()
        {
            Task<string> nameA = MethodA();
            Task<string> nameB = MethodB();
            var date = DateTime.Now.ToString("yyMMdd");
            string NameA = await nameA;
            string NameB = await nameB;
            var newdate = $"{DateTime.Now:yyMMdd}";
            return AjaxResult.Success(date + "--" + NameA + "---" + NameB + "--" + newdate);

            //string date = DateTime.Now.ToString();
            //string nameA = await MethodA();
            //string nameB = await MethodB();
            //string newdate = DateTime.Now.ToString();
            //return AjaxResult.Success(date+"--"+nameA+"---"+nameB+"--"+newdate);
        }
        [HttpGet]
        [Route("Index1")]
        public async Task<AjaxResultModel<string>> Index1()
        {
            string date = DateTime.Now.ToString();
            string nameA = await MethodA();
            string nameB = await MethodB();
            string newdate = DateTime.Now.ToString();
            return AjaxResult.Success(date + "--" + nameA + "---" + nameB + "--" + newdate);
        }
        public async Task<string> MethodA()
        {
            return await Task.Run(() =>
            {
                Thread.Sleep(5000);
                return $"我是MethodA返回值{DateTime.Now.ToString()}";
            });
        }
        public async Task<string> MethodB()
        {
            return await Task.Run(() =>
            {
                Thread.Sleep(3000);
                return $"我是MethodB返回值{DateTime.Now.ToString()}";
            });
        }
    }
}