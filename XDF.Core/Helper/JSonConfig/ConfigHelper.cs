using System;
using System.Collections.Generic;
using System.IO;
using XDF.Core.Helper.Ext;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
namespace XDF.Core.Helper.JsonConfig

{
    public class JsonConfigHelper
    {
        private static Dictionary<string, string> _mySqlconnection;
        private static Dictionary<string, string> _sqlServerconnection;
        private static Dictionary<string, string> _apiurl;
        private static DbConnectionConfig _dBConnectionConfig;
        private static string _mongo;
        private static string _redisStr;

        private static string _rabbitMqStr;
        /// <summary>
        /// app环境
        /// </summary>
        private static string _environment;
        /// <summary>
        /// 短信渠道
        /// </summary>
        private static string _smsChannel;
        private static IConfigurationRoot _configurationRoot;
        private static readonly object Lock = new object();
        public static IConfigurationRoot ConfigurationRoot
        {
            get
            {
                if (_configurationRoot == null)
                {
                    lock (Lock)
                    {
                        if (_configurationRoot == null)
                        {
                            var baseDir = AppContext.BaseDirectory;
                            if (!File.Exists(baseDir + "/Config/appsettings.json"))
                            {
                                var di = new DirectoryInfo(string.Format("{0}../../../", baseDir));
                                baseDir = di.FullName;
                                if (!File.Exists(baseDir + "/Config/appsettings.json"))
                                {
                                    throw new Exception("没有找到配置文件");
                                }
                            }
                            _configurationRoot = new ConfigurationBuilder().SetBasePath(baseDir).Add(new JsonConfigurationSource
                            {
                                Path = "/Config/appsettings.json",
                                Optional = false,
                                ReloadOnChange = true
                            }).Build();
                        }
                    }
                }
                return _configurationRoot;
            }
        }
        /// <summary>
        ///     取得数据库链接配置
        /// </summary>
        public static DbConnectionConfig GetDbConnectionStr(string dbName)
        {
            if (_dBConnectionConfig == null)
            {
                lock (Lock)
                {
                    if (_dBConnectionConfig == null)
                    {
                        var conn = ConfigurationRoot.GetSection(dbName);
                        _dBConnectionConfig = new DbConnectionConfig()
                        {
                            ConnectionString = conn.GetSection("connectionstring").Value,
                            Type =Enum.Parse<DbType>(conn.GetSection("type").Value.ToLower()) 
                        };
                    }
                }
            }
            return _dBConnectionConfig;
        }
        /// <summary>
        ///     取得数据库链接配置
        /// </summary>
        public static Dictionary<string, string> GetMySqlConnectionStr
        {
            get
            {
                if (_mySqlconnection == null)
                {
                    lock (Lock)
                    {
                        if (_mySqlconnection == null)
                        {
                            var conn = ConfigurationRoot.GetSection("MySql");
                            _mySqlconnection = new Dictionary<string, string>
                            {
                                {"YSKJ", conn.GetSection("YSKJ").Value},
                            };
                        }
                    }
                }
                return _mySqlconnection;
            }
        }
        /// <summary>
        ///     取得数据库链接配置
        /// </summary>
        public static Dictionary<string, string> GetSqlServerConnectionStr
        {
            get
            {
                if (_sqlServerconnection == null)
                {
                    lock (Lock)
                    {
                        if (_sqlServerconnection == null)
                        {
                            var conn = ConfigurationRoot.GetSection("SqlServer");
                            _sqlServerconnection = new Dictionary<string, string>
                            {
                                {"crm", conn.GetSection("Crm").Value},
                                {"XDF", conn.GetSection("XDF").Value},
                            };
                        }
                    }
                }
                return _sqlServerconnection;
            }
        }
        /// <summary>
        ///     apiURL
        /// </summary>
        public static Dictionary<string, string> GetApiUrlStr
        {
            get
            {
                if (_apiurl == null)
                {
                    lock (Lock)
                    {
                        if (_apiurl == null)
                        {
                            var conn = ConfigurationRoot.GetSection("ApiUrl");
                            _apiurl = new Dictionary<string, string>
                            {
                                {"CommonApi", conn.GetSection("CommonApi").Value},
                            };
                        }
                    }
                }
                return _apiurl;
            }
        }
        public static string GetAppRedisStr
        {
            get
            {
                if (_redisStr.IsStringEmpty())
                {
                    lock (Lock)
                    {
                        if (_redisStr.IsStringEmpty())
                        {
                            _redisStr = ConfigurationRoot.GetSection("Redis").Value;
                        }
                    }
                }
                return _redisStr;
            }
        }
        public static string GetAppMongoStr
        {
            get
            {
                if (_mongo == null)
                {
                    lock (Lock)
                    {
                        if (_mongo == null)
                        {
                            _mongo = ConfigurationRoot.GetSection("Mongo").Value;
                        }
                    }
                }
                return _mongo;
            }
        }
        public static string GetRabbitMqStr
        {
            get
            {
                if (_rabbitMqStr.IsStringEmpty())
                {
                    lock (Lock)
                    {
                        if (_rabbitMqStr.IsStringEmpty())
                        {
                            _rabbitMqStr = ConfigurationRoot.GetSection("RabbitMq").Value;
                        }
                    }
                }
                return _rabbitMqStr;
            }
        }
        public static bool GetEnvironment
        {
            get
            {
                if (_environment == null)
                {
                    lock (Lock)
                    {
                        if (_environment.IsStringEmpty())
                        {
                            _environment = ConfigurationRoot.GetSection("Environment").Value;
                        }
                    }
                }
                return _environment == "release";
            }
        }
        public static string GetSmsChannel
        {
            get
            {
                if (_smsChannel == null)
                {
                    lock (Lock)
                    {
                        if (_smsChannel.IsStringEmpty())
                        {
                            _smsChannel = ConfigurationRoot.GetSection("SmsChannel").Value;
                        }
                    }
                }
                return _smsChannel;
            }
        }
    }
}