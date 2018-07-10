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
    public class EmployeeController : ControllerBase
    {
        [HttpGet]
        public PageListModel<EmployeeEntity> Get([FromQuery]PageListModel<EmployeeEntity> info)
        {
            var res = EmployeeService.Instance.Filter(info);
            return res;
        }
        [HttpGet("{id}")]
        public EmployeeEntity Get(int id)
        {
            //根据主键获取一条记录
            return EmployeeService.Instance.Find(1);
        }

        public EmployeeEntity Hah()
        {
            return  new EmployeeEntity();
        }
    }
}