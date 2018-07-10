using DotnetSpider.Core;
using DotnetSpider.Core.Pipeline;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XDF.Console
{
    public class DetailPipeline : IPipeline
    {
        private string path;
        public DetailPipeline(string _path)
        {

            if (string.IsNullOrEmpty(_path))
            {
                throw new Exception("路径不能为空！");
            }
            path = _path;
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
        }
        public void Dispose()
        {

        }

        public void Init()
        {
            
        }

        public void Process(ResultItems resultItems, ISpider spider)
        {
            
        }

        public void Process(IEnumerable<ResultItems> resultItems, ISpider spider)
        {
            foreach (var resultItem in resultItems)
            {
                Cnblog cnblog = resultItem.Results["detail"];
                FileStream fs = File.Create(path + "\\" + cnblog.Title.Substring(0,3) + ".txt");
                byte[] bytes = UTF8Encoding.UTF8.GetBytes("Url:" + cnblog.Url + Environment.NewLine + cnblog.Content);
                fs.Write(bytes, 0, bytes.Length);
                fs.Flush();
                fs.Close();
            }
          
        }
    }
}
