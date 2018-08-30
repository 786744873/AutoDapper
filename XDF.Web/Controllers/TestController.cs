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
        public string LastName { get; set; }
    }
    public class TestController : BaseController
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var settings = new ConnectionConfiguration(new Uri("http://localhost:9200")).RequestTimeout(TimeSpan.FromMinutes(2));

            var lowlevelClient = new ElasticLowLevelClient(settings);
            var person = new Person
            {
                FirstName = "Martijn",
                LastName = "Laarman"
            };

            var indexResponse = lowlevelClient.Index<byte[]>("person", "1",PostData<Person>);
            byte[] responseBytes = indexResponse.bo;

            var asyncIndexResponse = await lowlevelClient.IndexAsync<string>("people", "person", "1", person);
            string responseString = asyncIndexResponse.Body;
        }
    }
}