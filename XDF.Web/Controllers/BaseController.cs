using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XDF.Core.Helper.Ajax;
using XDF.Core.Helper.Log;

namespace XDF.Web.Controllers
{
    /// <summary>
    /// 父类，所有controller都要继承base
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : Controller
    {
        /// <summary>
        /// 重写执行action之前调用方法
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            using (var body=new StreamReader(context.HttpContext.Request.Body))
            {
                string content = body.ReadToEnd();
            }
            var argDic = context.ActionArguments;
            if (argDic.Any())
            {
                LogHelper.Info(argDic.ToJson());
            }
            base.OnActionExecuting(context);
        }
    }
}