using System;
using Xunit;
using Newtonsoft.Json;
using XDF.Core.Entity;
using XDF.Core.Helper.Mongo.Base;
using XDF.Core.Model;
using XDF.Service;

namespace XDF.Test
{
    public class DapperTest
    {
        [Fact]
        public void FindById()
        {
            var res = UserService.Instance.Find(8);
        }
        [Fact]
        public void FindByWhere()
        {
            var res = UserService.Instance.Find("Id=@id", new { id = 1 });
        }
        [Fact]
        public void Filter()
        {
            var res = UserService.Instance.Filter();
        }
        [Fact]
        public void FilterByWhere()
        {
            var res = UserService.Instance.Filter("Id>@id", new { id = 1 });
        }
        [Fact]
        public void FilterByOrderBy()
        {
            var res = UserService.Instance.Filter("Id>@id", new { id = 1 }, "Id", "Id,RealName");
        }
        [Fact]
        public void FilterByPageList()
        {
            PageListModel<UserEntity> page = new PageListModel<UserEntity>();
            var res = UserService.Instance.Filter(page);
        }
        [Fact]
        public void Insert()
        {
            for (int i = 0; i < 100; i++)
            {
                UserService.Instance.Insert(new UserEntity() { RealName = "任富帅" + i, Mobile = "18530812106", LoginName = "任富帅" + i, Email = "renfushuai@xdf.cn" });
            }
        }
        [Fact]
        public void Edit()
        {
            var user = UserService.Instance.Find(8);
            user.RealName = "富帅滴滴";
            var res = UserService.Instance.Edit(user);
            UserService.Instance.Edit("Id>@id", "RealName=@realName", new { realName = "ewerwwqr", id = 100 });
        }
        [Fact]
        public void Del()
        {
            UserService.Instance.Del(2);
            UserService.Instance.Del("Id>@id", new { id = 100 });
        }
        [Fact]
        public void Count()
        {
            var res = UserService.Instance.Count("Id>@id", new { id = 10 });
        }
        [Fact]
        public async void FindAsync()
        {
            var res = await UserService.Instance.FindAsync(8);
        }
    }
}
