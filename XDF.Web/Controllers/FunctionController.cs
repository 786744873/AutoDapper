using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace XDF.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FunctionController : ControllerBase
    {
        [HttpGet]
        [Route("GetAllFunctionList")]
        public ActionResult<string> GetAllFunctionList(string where)
        {
            return where;
        }
        [HttpGet]
        public ActionResult<int> GetDetail(int id)
        {
            return Ok(id);
        }
      
    }
}