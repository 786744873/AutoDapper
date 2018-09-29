using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XDF.Web.Controllers
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; } = "富帅";
    }
    public class TestController : BaseController
    {
        [HttpPost]
        [Route("GetPerson")]
        public Person Get(Person p)
        {
            return p;
        }
    }
}