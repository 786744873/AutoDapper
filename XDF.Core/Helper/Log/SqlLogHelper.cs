using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace XDF.Core.Helper.Log
{
   public class SqlLogHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Info(string message)
        {
            Logger.Info(message);
        }
    }
}
