using DotnetSpider.Core;
using DotnetSpider.Core.Pipeline;
using DotnetSpider.Core.Processor;
using DotnetSpider.Core.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace XDF.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 博客园数据抓取
            //var site = new Site() { EncodingName = "UTF-8" };
            //for (int i = 1; i <= 30; i++)//30页
            //{
            //    site.AddStartUrl(
            //        $"http://www.cnblogs.com/p{i}");//已更正去掉#号，本来是"http://www.cnblogs.com/#p{i}",这样发现请求的是http://www.cnblogs.com
            //}

            //Spider spider = Spider.Create(site, new PageListProcessor()).AddPipeline(new ListPipeline("test.json"));//两个线程
            //spider.ThreadNum = 2;
            //spider.Run(); 
            #endregion
            #region 汽车logo抓取
            //var site = new Site()
            //{
            //    CycleRetryTimes = 1,
            //    SleepTime = 200,
            //    //  DownloadFiles = true,
            //    //Headers = new Dictionary<string, string>()
            //    //{
            //    //    { "Accept","text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8" },
            //    //    { "Cache-Control","no-cache" },
            //    //    { "Connection","keep-alive" },
            //    //    { "Content-Type","application/x-www-form-urlencoded; charset=UTF-8" },
            //    //    { "User-Agent","Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.139 Safari/537.36"}
            //    //}
            //};
            //site.AddStartUrl("https://car.m.autohome.com.cn/");
            //var spider = Spider.Create(site, new LogoInfoProcessor()).AddPipeline(new LogoInfoPipe());
            //spider.ThreadNum = 2;
            //spider.Run();
            //System.Console.Read(); 
            #endregion

            var site = new Site
            {
                CycleRetryTimes = 1,
                SleepTime = 200,
                Headers = new Dictionary<string, string>()
                {
                    { "Accept","text/html, */*; q=0.01" },
                    { "Referer", "https://store.mall.autohome.com.cn/83106681.html"},
                    { "Cache-Control","no-cache" },
                    { "Connection","keep-alive" },
                    { "Content-Type","application/x-www-form-urlencoded; charset=UTF-8" },
                    { "User-Agent","Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.167 Safari/537.36"}
                }
            };
            List<Request> resList = new List<Request>();
            for (int i = 1; i <= 33; i++)
            {
                Request res = new Request
                {
                    PostBody =$"id=7&j=%7B%22createMan%22%3A%2218273159100%22%2C%22createTime%22%3A1518433690000%2C%22row%22%3A5%2C%22siteUserActivityListId%22%3A8553%2C%22siteUserPageRowModuleId%22%3A84959%2C%22topids%22%3A%22%22%2C%22wherePhase%22%3A%221%22%2C%22wherePreferential%22%3A%220%22%2C%22whereUsertype%22%3A%220%22%7D&page={i}&shopid=83106681",
                    Url = "https://store.mall.autohome.com.cn/shop/ajaxsitemodlecontext.jtml",
                    Method = System.Net.Http.HttpMethod.Post
                };
                resList.Add(res);
            }
            var spider = Spider.Create(site, new QueueDuplicateRemovedScheduler(), new AutoHomeProcessor())
                .AddStartRequests(resList.ToArray())
                .AddPipeline(new AutoHomePipe());
            spider.ThreadNum = 1;
            spider.Run();
            System.Console.Read();
        }
    }

}
