using System;
using System.Collections.Generic;
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
    public class ConfigController : ControllerBase
    {
        [HttpGet]
        public PageListModel<ConfigEntity> Get([FromQuery]PageListModel<ConfigEntity> info)
        {
            var res = ConfigService.Instance.Filter(info);
            return res;
        }
        [HttpGet("{id}")]
        public AjaxResultModel<ConfigEntity> Get(int id)
        {
            var model = ConfigService.Instance.Find(id);
            if (model==null)
            {
                return AjaxResult.Error<ConfigEntity>("参数错误");
            }
            return AjaxResult.Success(model);
        }
        [HttpPost]
        public AjaxResultModel<string> Post([FromBody]ConfigEntity model)
        {
            if (ConfigService.Instance.Count("SName=@SName",new {model.SName })>0)
            {
                return AjaxResult.Error("已存在相同的Key");
            }
            return AjaxResult.Success("添加成功");
        }
    }
}