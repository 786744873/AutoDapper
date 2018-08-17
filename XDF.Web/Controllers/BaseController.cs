using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XDF.Core.Helper.Log;

namespace XDF.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
          var argDic=  context.ActionArguments;
           LogHelper.Info(argDic.ToJson());
           base.OnActionExecuting(context);
        }
    }
}