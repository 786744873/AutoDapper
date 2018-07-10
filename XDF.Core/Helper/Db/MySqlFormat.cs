using System;
using System.Collections.Generic;
using System.Text;

namespace XDF.Core.Helper.Db
{
    public static class MySqlFormat
    {
        public const string Insert = "INSERT INTO {0}({1}) VALUES ({2}) ;SELECT  @@IDENTITY";
        public const string Find = "SELECT  {0} FROM {1} {2} LIMIT 1";
        public const string Filter = "SELECT {0},{1} FROM {2}";
        public const string Update = "UPDATE {0} SET {1}";
        public const string Del = "DELETE FROM {0}";
        public const string Count = "SELECT COUNT(1) FROM {0}";
        public const string Single = "SELECT {0} FROM {1}";
    }
}
