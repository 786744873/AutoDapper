using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using XDF.Core.Helper.Mongo;
using XDF.Core.Helper.Mongo.Base;
using XDF.Core.Helper.Tools;

namespace XDF.Web.Controllers
{
    [Mongo("api", "apiLog")]
    public class User : MongoEntity
    {
        public string Name { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime BirthDateTime { get; set; }

        public int Sex { get; set; }
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
        [HttpGet]
        [Route("TestMongo")]
        public ActionResult<User> TestMongo()
        {
            var url = "";
            var mongoRepository = new MongoRepository(url);

            var u = new User
            {
                Name = "renfushuai",
                BirthDateTime = new DateTime(1991, 2, 2),
                Sex = 1
            };
            var addresult = mongoRepository.Add(u);

            User getResult = mongoRepository.Get<User>(a => a.Id == u.Id);
            return getResult;
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
