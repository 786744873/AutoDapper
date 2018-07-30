using DotnetSpider.Core;
using DotnetSpider.Core.Processor;
using System;
using System.Collections.Generic;
using System.Text;

namespace XDF.Console
{
    public class AutoHomeProcessor : BasePageProcessor
    {
        protected override void Handle(Page page)
        {
            List<AutoHomeShopListEntity> list = new List<AutoHomeShopListEntity>();
            var modelHtmlList = page.Selectable.XPath(".//div[@class='list']/ul[@class='fn-clear']/li[@class='carbox']").Nodes();
            foreach (var modelHtml in modelHtmlList)
            {
                AutoHomeShopListEntity entity = new AutoHomeShopListEntity();
                entity.ShopId = 83106681;
                entity.DetailUrl = modelHtml.XPath(".//a/@href").GetValue();
                entity.CarImg = modelHtml.XPath(".//a/div[@class='carbox-carimg']/img/@src").GetValue();
                var price = modelHtml.XPath(".//a/div[@class='carbox-info']").GetValue(DotnetSpider.Core.Selector.ValueOption.InnerText).Trim().Replace(" ", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).TrimStart('¥').Split("¥");
                if (price.Length > 1)
                {
                    entity.Price = price[0];
                    entity.DelPrice = price[1];
                }
                else
                {
                    entity.Price = price[0];
                    entity.DelPrice = price[0];
                }
                entity.Title = modelHtml.XPath(".//a/div[@class='carbox-title']").GetValue();
                entity.Tip = modelHtml.XPath(".//a/div[@class='carbox-tip']").GetValue();
                entity.BuyNum = modelHtml.XPath(".//a/div[@class='carbox-number']/span").GetValue();
                list.Add(entity);

            }
            page.AddResultItem("CarList", list);

        }
    }
}
