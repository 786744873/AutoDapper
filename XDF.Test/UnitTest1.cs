using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Newtonsoft.Json;

namespace XDF.Test
{
    public class StudentInfo:Person
    {
        public StudentInfo()
        {
            Name = "student";
            Age = 18;
        }
        [JsonIgnore]
        public DateTime Birthday { get; set; }
    }

    public class Teacher : Person
    {
        public Teacher()
        {
            Name = "teacher";
            Age = 10;
        }
       
    }
    public class Person
    {
        public Person()
        {

        }
        [JsonProperty("sName")]
        public  static string Name { get; set; }
        public  static  int Age { get; set; }

        public static int GetAge()
        {
            return Age;
        }
        public static string GetName()
        {
            return Name;
        }
    }
    public class UnitTest1
    {
        [Fact]
        public  void Test1()
        {
            //var stu = new StudentInfo { Birthday = DateTime.Now};
            //var json = JsonConvert.SerializeObject(stu);
            //StudentInfo model = JsonConvert.DeserializeObject<StudentInfo>(json);
            //var sAge= new StudentInfo().GetName();
            //  var tAge =new  Teacher().GetName();

            var tAge = Teacher.GetName();
            var sAge = StudentInfo.GetName();
            List<string> list = null;
            
            //list.ForEach(m =>
            //{

            //});
            foreach (var item in list)
            {
                
            }

        }

        [Fact]
        public void Test2()
        {
          Student s=new Student();
            s.Name += "23423";

        }
    }
}
