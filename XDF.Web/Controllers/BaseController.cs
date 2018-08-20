using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XDF.Core.Helper.Ajax;
using XDF.Core.Helper.Log;

namespace XDF.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            int s = 123_456;
            var argDic = context.ActionArguments;
            LogHelper.Info(argDic.ToJson());
            var stuInfo = GetStuInfo(19);
            LogHelper.Info(stuInfo.age.ToString());
            LogHelper.Info(stuInfo.name);
            base.OnActionExecuting(context);
        }

        public (string name, int age) GetStuInfo(long id)
        {
            return ("xiaoming", 18);
        }
    }
}