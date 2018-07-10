using DotnetSpider.Core;
using DotnetSpider.Core.Pipeline;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace XDF.Console
{
    public class LogoInfoPipe : BasePipeline
    {
        public override void Process(IEnumerable<ResultItems> resultItems, ISpider spider)
        {
            foreach (var resultItem in resultItems)
            {
                var logoInfoList = resultItem.GetResultItem("LogoInfoList") as List<LogoInfoModel>;
                foreach (LogoInfoModel model in resultItem.GetResultItem("LogoInfoList"))
                {
                    System.Console.WriteLine($"名称：{model.BrandName}，图片地址：{model.ImgPath}");
                    SaveFile(model.ImgPath, model.BrandName);
                }
            }
        }
        public void SaveFile(string url,string fileName)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.RequestUri = new Uri(url);
            httpRequestMessage.Method = HttpMethod.Get;
            HttpClient httpClient = new HttpClient();
            var httpResponse = httpClient.SendAsync(httpRequestMessage);
            string filePath = Environment.CurrentDirectory + "/img/" + fileName + ".jpg";

            if (!File.Exists(filePath))
            {
                string folder = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                File.WriteAllBytes(filePath, httpResponse.Result.Content.ReadAsByteArrayAsync().Result);
            }
        }
    }
}
