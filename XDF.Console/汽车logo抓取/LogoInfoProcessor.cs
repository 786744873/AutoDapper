using DotnetSpider.Core;
using DotnetSpider.Core.Processor;
using System;
using System.Collections.Generic;
using System.Text;

namespace XDF.Console
{
    public class LogoInfoProcessor : BasePageProcessor
    {
        protected override void Handle(Page page)
        {
            List<LogoInfoModel> logoInfoList = new List<LogoInfoModel>();
            var logoInfoNodes= page.Selectable.XPath(".//div[@id='div_ListBrand']//div[@class='item']").Nodes();
            foreach (var logoInfo in logoInfoNodes)
            {
                var model = new LogoInfoModel();
                model.BrandName = logoInfo.XPath("./span").GetValue();
                model.ImgPath = logoInfo.XPath("./img/@src").GetValue();
                if (model.ImgPath == null)
                {
                    model.ImgPath = logoInfo.XPath("./img/@data-src").GetValue();
                }
                if (model.ImgPath.IndexOf("https") == -1)
                {
                    model.ImgPath = "https:" + model.ImgPath;
                }
                logoInfoList.Add(model);
            }
            page.AddResultItem("LogoInfoList", logoInfoList);
        }
    }
}
