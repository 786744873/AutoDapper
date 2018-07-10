using DotnetSpider.Core;
using DotnetSpider.Core.Pipeline;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XDF.Console
{
    public class ListPipeline : IPipeline
    {
        private string _path;

        public ListPipeline(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new Exception("文件名不能为空！");
            }

            _path = path;

            if (!File.Exists(_path))
            {
                File.Create(_path);
            }
        }
        public void Dispose()
        {
        }

        public void Init()
        {
        }

        public void Process(IEnumerable<ResultItems> resultItems, ISpider spider)
        {
            lock (this)
            {
                var site = new Site() { CycleRetryTimes = 3, EncodingName = "UTF-8" };
                foreach (var resultItem in resultItems)
                {
                    foreach (Cnblog item in resultItem.GetResultItem("Result"))
                    {
                        File.AppendAllText(_path, JsonConvert.SerializeObject(item));
                        site.AddStartUrl(item.Url);
                        //RequestDetail(item);
                    }
                    RequestDetail(site);
                }
            }
        }
        /// <summary>
        /// 请求详细页
        /// </summary>
        /// <param name="entry"></param>
        private static void RequestDetail(Site site)
        {
           
            Spider spider = Spider.Create(site, new PageDetailProcessor()).AddPipeline(new DetailPipeline("details"));
            spider.ThreadNum = 5;
            spider.Run();
        }
    }
}
