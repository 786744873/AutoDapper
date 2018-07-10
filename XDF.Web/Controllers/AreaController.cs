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
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        [HttpGet]
        public List<dynamic> Get()
        {
            var allAreaList = AreaNewService.Instance.Filter();
            dynamic dyRoot = new ExpandoObject();
            dyRoot.id = 0;
            dyRoot.name = "根节点";
            dyRoot.code = "0";
            CreateAreaTree(allAreaList, dyRoot);
            return dyRoot.children;
        }
        private void CreateAreaTree(List<AreaNewEntity> all,dynamic item)
        {
            var areaList = all.Where(m => m.SFatherCode == item.code).ToList();
            List<dynamic> list = new List<dynamic>();
            areaList.ForEach(m =>
            {
                dynamic dy = new ExpandoObject();
                dy.id = m.ID;
                dy.name = m.SName;
                dy.code = m.SCode;
                list.Add(dy);
            });
            if (list.Any())
            {
                item.children = list;
            }
            list.ForEach(m =>
            {
                CreateAreaTree(all, m);
            });
        }
    }
}