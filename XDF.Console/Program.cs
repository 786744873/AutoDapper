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
            var site = new Site()
            {
                CycleRetryTimes = 1,
                SleepTime = 200,
              //  DownloadFiles = true,
                //Headers = new Dictionary<string, string>()
                //{
                //    { "Accept","text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8" },
                //    { "Cache-Control","no-cache" },
                //    { "Connection","keep-alive" },
                //    { "Content-Type","application/x-www-form-urlencoded; charset=UTF-8" },
                //    { "User-Agent","Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.139 Safari/537.36"}
                //}
            };
            site.AddStartUrl("https://car.m.autohome.com.cn/");
            var spider = Spider.Create(site, new LogoInfoProcessor()).AddPipeline(new LogoInfoPipe());
            spider.ThreadNum = 2;
            spider.Run();
            System.Console.Read();
        }
    }

}
