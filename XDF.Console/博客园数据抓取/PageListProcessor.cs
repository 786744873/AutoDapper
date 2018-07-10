using DotnetSpider.Core;
using DotnetSpider.Core.Processor;
using DotnetSpider.Core.Selector;
using System;
using System.Collections.Generic;
using System.Text;

namespace XDF.Console
{
    public class PageListProcessor : BasePageProcessor
    {
        public Site Site { get; set; }
        protected override void Handle(Page page)
        {
            var totalCnblogElements = page.Selectable.SelectList(Selectors.XPath("//div[@class='post_item']")).Nodes();
            List<Cnblog> results = new List<Cnblog>();
            foreach (var cnblogElement in totalCnblogElements)
            {
                var cnblog = new Cnblog();
                cnblog.Title = cnblogElement.Select(Selectors.XPath(".//div[@class='post_item_body']/h3/a")).GetValue();
                cnblog.Url = cnblogElement.Select(Selectors.XPath(".//div[@class='post_item_body']/h3")).Links().GetValue();
                cnblog.Author = cnblogElement.Select(Selectors.XPath(".//div[@class='post_item_foot']/a[1]")).GetValue();
                results.Add(cnblog);
            }
            page.AddResultItem("Result", results);
        }
        
    }
}
