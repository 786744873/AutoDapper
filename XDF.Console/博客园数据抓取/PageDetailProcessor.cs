using DotnetSpider.Core;
using DotnetSpider.Core.Processor;
using DotnetSpider.Core.Selector;
using System;
using System.Collections.Generic;
using System.Text;

namespace XDF.Console
{
    public class PageDetailProcessor : BasePageProcessor
    {
        protected override void Handle(Page page)
        {
            Cnblog cnblog = new Cnblog();
            cnblog.Title = page.Selectable.Select(Selectors.XPath("//a[@id='cb_post_title_url']")).GetValue();
            cnblog.Content = page.Selectable.Select(Selectors.XPath("//*[@id='cnblogs_post_body']")).GetValue();
            cnblog.Url = page.Url;
            page.AddResultItem("detail", cnblog);
        }
    }
}
