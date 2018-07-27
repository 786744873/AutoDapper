using System;
using System.Collections.Generic;
using System.Text;

namespace XDF.Console
{
    public class AutoHomeShopListEntity
    {
        public string DetailUrl { get; set; }

        public string CarImg { get; set; }

        public string Price { get; set; }

        public string DelPrice { get; set; }

        public string Title { get; set; }

        public string Tip { get; set; }

        public string BuyNum { get; set; }
        public override string ToString()
        {
            return $"{Title}|{Price}|{DelPrice}|{BuyNum}";
        }
    }
}
