using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using Xunit;
using Newtonsoft.Json;
using XDF.Core.Helper.Mongo;
using XDF.Core.Helper.Mongo.Base;

namespace XDF.Test
{

    public class MongoDbTest
    {
        [Fact]
        public void AddStudent()
        {
            var result = MongoRepository.Instance.Add(new Student()
            {
                Age = 111,
                Name = "xiaoming"
            });
            Assert.True(result);
        }

        #region Add
        [Fact]
        public void Add_Normal_True()
        {
            var result = MongoRepository.Instance.Add(new User
            {
                Age = 111,
                Name = "chengongeee"
            });

            var result2 = MongoRepository.Instance.Add(new User
            {
                Age = 111,
                Name = "chengong100"
            });

            Assert.True(result);
            Assert.True(result2);
        }

        #endregion

        #region Update

        [Fact]
        public void Update_Normal_True()
        {
            var user = new User
            {
                Age = 111,
                Name = "chengongeee"
            };
            MongoRepository.Instance.Add(user);

            user.Name = "updateName";
            user.NumList = new List<int>();

            user.Sons = new List<User> { new User { Id = "1231", Name = "aads" }, new User { Id = "123134", Name = "aads" } };

            user.AddressList = new List<string> { "123123", "asdsdf" };

            var result = MongoRepository.Instance.Update(user);

            Assert.True(result);
        }

        [Fact]
        public void Update_Where_True()
        {
            var user = new User
            {
                Age = 111,
                Name = "chengongeee"
            };
            MongoRepository.Instance.Add(user);

            var qwe = new List<User> { new User { Id = "1231", Name = "aads" }, new User { Id = "123134", Name = "aads" } };

            var qwe2 = new List<string> { "123123", "asdsdf" };

            var result = MongoRepository.Instance.Update<User>(a => a.Id == user.Id, a => new User { AddressList = new List<string> { "123123", "asdsdf" } });

            Assert.True(result > 0);
        }

        #endregion

        #region Update

        [Fact]
        public void Delete_Normal_True()
        {
            var user = new User
            {
                Age = 111,
                Name = "chengongeee"
            };
            MongoRepository.Instance.Add(user);

            var result = MongoRepository.Instance.Delete(user);

            Assert.True(result);
        }

        [Fact]
        public void Delete_Where_True()
        {
            var user = new User
            {
                Age = 111,
                Name = "chengongeee"
            };
            MongoRepository.Instance.Add(user);

            var result = MongoRepository.Instance.Delete<User>(a => a.Id == user.Id);

            Assert.True(result > 0);
        }

        #endregion

        #region Get
        [Fact]
        public void Get_Normal_True()
        {
            var skychen = MongoRepository.Instance.Get<User>(a => true);

            Assert.NotNull(skychen);
        }

        [Fact]
        public void Get_Selector_True()
        {
            var skychenName = MongoRepository.Instance.Get<User, string>(a => true, a => a.Name);

            Assert.NotNull(skychenName);
        }

        [Fact]
        public void Get_OrderBy_True()
        {
            var skychenName = MongoRepository.Instance.Get<User>(a => true, a => a.Desc(b => b.Age));

            Assert.NotNull(skychenName);
        }

        [Fact]
        public void Get_SelectorOrderBy_True()
        {
            var skychenName = MongoRepository.Instance.Get<User, User>(a => true, a => new User { Name = a.Name, Sex = a.Sex }, a => a.Desc(b => b.Age));

            Assert.NotNull(skychenName.Name);
        }
        #endregion

        #region ToList
        [Fact]
        public void ToList_Normal_True()
        {
            var skychen = MongoRepository.Instance.Filter<User>(a => a.Name== "updateName");

            Assert.True(skychen.Any());
        }

        [Fact]
        public void ToList_Top_True()
        {
            var skychen = MongoRepository.Instance.Filter<User>(a => true, 10);

            Assert.Equal(skychen.Count, 10);
        }

        [Fact]
        public void ToList_Orderby_True()
        {
            var skychen = MongoRepository.Instance.Filter<User>(a => true, a => a.Desc(b => b.Age).Desc(b => b.Sex), 10);

            Assert.Equal(skychen.Count, 10);
        }

        [Fact]
        public void ToList_Selector_True()
        {
            var skychen = MongoRepository.Instance.ToList<User, string>(a => true, a => a.Name);

            Assert.True(skychen.Any());
        }

        [Fact]
        public void ToList_Selector_Orderby_True()
        {
            var skychen = MongoRepository.Instance.ToList<User, string>(a => true, a => a.Name, a => a.Desc(b => b.Name));

            Assert.True(skychen.Any());
        }

        [Fact]
        public void ToList_Selector_Orderby_Top_True()
        {
            var skychen = MongoRepository.Instance.ToList<User, string>(a => true, a => a.Name, a => a.Desc(b => b.Name), 100);

            Assert.True(skychen.Any());
        }

        #endregion

        #region PageList
        [Fact]
        public void PageList_Normal_True()
        {
            var skychen = MongoRepository.Instance.PageList<User>(a => true, 1, 10);

            Assert.Equal(10, skychen.Items.Count);
            Assert.Equal(true, skychen.HasNext);
            Assert.Equal(false, skychen.HasPrev);
        }

        [Fact]
        public void PageList_Orderby_True()
        {
            var skychen = MongoRepository.Instance.PageList<User>(a => true, a => a.Desc(b => b.Name), 2, 10);

            Assert.Equal(skychen.Items.Count, 10);
            Assert.Equal(skychen.HasNext, true);
            Assert.Equal(skychen.HasPrev, true);
        }

        [Fact]
        public void PageList_Selector_True()
        {
            var skychen = MongoRepository.Instance.PageList<User, string>(a => true, a => a.Name, 2, 10);

            Assert.Equal(skychen.Items.Count, 10);
            Assert.Equal(skychen.HasNext, true);
            Assert.Equal(skychen.HasPrev, true);
        }

        [Fact]
        public void PageList_Selector_OrderBy_True()
        {
            var skychen = MongoRepository.Instance.PageList<User, string>(a => true, a => a.Name, a => a.Desc(b => b.Name), 2, 20);

            Assert.Equal(skychen.Items.Count, 20);
            Assert.Equal(skychen.HasNext, true);
            Assert.Equal(skychen.HasPrev, true);
        }

        #endregion

        #region Exists

        [Fact]
        public void Exists_Normal_True()
        {
            var isExist = MongoRepository.Instance.Exists<User>(a => a.Name == "chengongeee");

            Assert.True(isExist);
        }

        [Fact]
        public void Exists_Normal_IsFalse()
        {
            var isExist = MongoRepository.Instance.Exists<User>(a => a.Name == "chengong100");

            Assert.False(isExist);
        }

        #endregion

        #region MyRegion

        [Fact]
        public void AddIfNotExist_Normal_True()
        {
            var result = MongoRepository.Instance.AddIfNotExist("chenggongUpdateTest", "UserInfo", new User
            {
                Age = 2222,
                Name = "chengongeee2222"
            });

            Assert.True(result);
        }
        #endregion
    }

    [Mongo("test", "User")]
    public class User : MongoEntity
    {
        public string Name { get; set; }

        public int Age { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime BirthDateTime { get; set; }

        public User Son { get; set; }

        public Sex Sex { get; set; }

        public List<int> NumList { get; set; }

        public List<string> AddressList { get; set; }


        public List<User> Sons { get; set; }
    }
    [Mongo("test2","Student")]
    public class Student: MongoEntity
    {
        public  string Name { get; set; }
        public int Age { get; set; }
    }

    public enum Sex
    {
        Man = 1,
        Woman = 2
    }

}
