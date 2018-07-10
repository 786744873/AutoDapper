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
    public class SchoolController : ControllerBase
    {
        [HttpGet]
        public PageListModel<SchoolEntity> Get([FromQuery]PageListModel<SchoolEntity> info)
        {
            var res = SchoolService.Instance.Filter(info);
            return res;
        }
    }
}