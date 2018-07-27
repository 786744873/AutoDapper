using System;
using Xunit;
using Newtonsoft.Json;

namespace XDF.Test
{
    public class StudentInfo
    {
        //自定义序列化的字段名称
        [JsonProperty("CName")]
        public string Name { get; set; }
        public int Age { get; set; }
        [JsonIgnore]
        public DateTime Birthday { get; set; }
    }
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var json=JsonConvert.SerializeObject(new StudentInfo { Name = "张三", Age = 18, Birthday = DateTime.Now });
        }
    }
}
