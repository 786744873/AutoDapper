using System;
using System.Collections.Generic;
using System.Text;

namespace XDF.Core.Helper.JsonConfig
{
   public class DbConnectionConfig
    {
        public string ConnectionString { get; set; }
        public DbType Type { get; set; }
    }

    public enum DbType
    {
        sqlserver,
        mysql
    }
}
