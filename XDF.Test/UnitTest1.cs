using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Newtonsoft.Json;
using XDF.Core.Validation;

namespace XDF.Test
{
    public class StudentInfo : Person
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
        public  string Name { get; set; }
        public  int Age { get; set; }
        [Price("价格不正确")]
        public  decimal Price { get; set; }
    
    }
    public class UnitTest1
    {
  

        [Fact]
        public void Test2()
        {
            Person p = new Person();
            p.Price = 10.0M;
            var res=p.Validation<Person, Person>();
        }
    }
}
