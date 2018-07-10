using System;
using Xunit;
using Newtonsoft.Json;

namespace XDF.Test
{
    public class StudentInfo
    {
        public string Name { get; set; }
        public int Age { get; set; }
       
        public DateTime Birthday { get; set; }
    }
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var json=JsonConvert.SerializeObject(new StudentInfo { Name = "уехЩ", Age = 18, Birthday = DateTime.Now });
        }
    }
}
