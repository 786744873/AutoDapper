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
    public class AutoHomePipe : BasePipeline
    {
        public override void Process(IEnumerable<ResultItems> resultItems, ISpider spider)

        {
            foreach (var resultItem in resultItems)
            {
                System.Console.WriteLine(((List<AutoHomeShopListEntity>) resultItem.Results["CarList"]).Count);
                foreach (var item in ((List<AutoHomeShopListEntity>) resultItem.Results["CarList"]))
                {
                    System.Console.WriteLine(item);
                }
            }
        }
    }
}
