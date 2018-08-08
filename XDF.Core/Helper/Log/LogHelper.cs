using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace XDF.Core.Helper.Log
{
    public class LogHelper
    {
       private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static void Debug(string txt)
        {
            Logger.Debug(txt);
        }
        public static void Info(string message)
        {
            Logger.Info(message);
        }
        public static void Error(string txt)
        {
            Logger.Error(txt);
        }
        public static void Sql(string sql)
        {
            SqlLogHelper.Info(sql);
        }
    }
}
