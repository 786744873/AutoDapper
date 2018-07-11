using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XDF.Core.Helper.Tools;

namespace XDF.Web.Controllers
{
    public class Student
    {
        public  string Name { get; set; }
        public  int Age { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var total = 200000;
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < total; i++)
            {
                NLogHelper.Info("nlog bigdata test: " + i);
            }
            sw.Stop();
            NLogHelper.Info($"total: {total}, Elapsed:{sw.ElapsedMilliseconds}");
            return "value"+10/id;
        }

        // POST api/values
        [HttpPost]
        public ActionResult<string> Post([FromBody]string loginName)
        {
            return loginName+"sss";
        }
        [HttpPost]
        [Route("getStudent")]
        public ActionResult<string> Post([FromBody] Student stu,string url)
        {
            return Extension.ToJson(stu)+url;
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
