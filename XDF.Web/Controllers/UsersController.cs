using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XDF.Core.Entity;
using XDF.Core.Helper.Ajax;
using XDF.Core.Model;
using XDF.Service;

namespace XDF.Web.Controllers
{
    public class UserController : BaseController
    {
       
        // GET api/values
        [HttpGet]
        [Route("GetAll")]
        public PageListModel<UserEntity> GetAll([FromQuery]PageListModel<UserEntity> info)
        {
            var res = UserService.Instance.Filter(info);
            return res;
        }
        [HttpGet]
        public AjaxResultModel<dynamic> Get(string token)
        {
            List<dynamic> menus = GetMenus();
            return AjaxResult.Success<dynamic>(new { name = "renfusuai", roles = new string[] { "admin" }, menus });
        }
        private List<dynamic> GetMenus()
        {
            List<FunctionEntity> menuList = FunctionService.Instance.Filter();
            List<dynamic> menus = new List<dynamic>();
            var parentsList = menuList.Where(f => f.IsNavigation == 1 && f.PId == 0).OrderByDescending(f => f.Sort).ToList();
            foreach (var pitem in parentsList)
            {
                dynamic pdy = new ExpandoObject();
                dynamic pmeta = new ExpandoObject();
                pmeta.title = pitem.Name;
                pmeta.icon = pitem.Icon;
                pdy.path = pitem.UrlPath;
                pdy.component = "layout";
                pdy.redirect = "noredirect";
                pdy.name = pitem.Name;
                pdy.meta = pmeta;
                var childers = menuList.Where(f => f.PId == pitem.Id).OrderByDescending(f => f.Sort).ToList();

                if (childers.Count > 0)
                {
                    List<dynamic> cmenus = new List<dynamic>();
                    foreach (var childerItem in childers)
                    {
                        dynamic cdy = new ExpandoObject();
                        dynamic cmeta = new ExpandoObject();
                        cmeta.title = childerItem.Name;
                        cmeta.icon = childerItem.Icon;
                        cmeta.noCache = true;
                        cdy.path = childerItem.UrlPath;
                        cdy.component = childerItem.ComponentPath;
                        cdy.name = childerItem.Name;
                        cdy.meta = cmeta;
                        cmenus.Add(cdy);
                    }
                    pdy.children = cmenus;
                }
                menus.Add(pdy);
            }
            return menus;
        }
        /// <summary>
        ///     登陆
        /// </summary>
        /// <param name="logiName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public AjaxResultModel<dynamic> Post(string loginName, string pwd)
        {
            return AjaxResult.Success<dynamic>("登录成功"+ loginName, new {token="123456789" });
        }
    }
}