using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Newtonsoft.Json;

namespace XDF.Test
{
    public class StudentInfo
    {
        //�Զ������л����ֶ�����
        [JsonProperty("sName")]
        public string Name { get; set; }
        [JsonProperty("sAge")]
        public int Age { get; set; }
        [JsonIgnore]
        public DateTime Birthday { get; set; }
    }
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var json=JsonConvert.SerializeObject(new StudentInfo { Name = "����", Age = 18, Birthday = DateTime.Now });
            StudentInfo model= JsonConvert.DeserializeObject<StudentInfo>(json);
        }

        [Fact]
        public void Test2()
        {
          Student s=new Student();
            s.Name += "23423";

        }
    }
}
