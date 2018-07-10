#region
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using XDF.Core.Helper.JsonConfig;
using StackExchange.Redis;
#endregion
namespace XDF.Core.Helper.Redis
{
    /// <summary>
    ///     Redis 助手
    /// </summary>
    public class RedisHelper
    {
        /// <summary>
        ///     redis 连接对象
        /// </summary>
        private static IConnectionMultiplexer _connMultiplexer;
        /// <summary>
        ///     锁
        /// </summary>
        private static readonly object Locker = new object();
        /// <summary>
        ///     数据库
        /// </summary>
        private readonly IDatabase _db;
        public static RedisHelper InstanceRedis => new RedisHelper(0);
        public static ConfigurationOptions Options
        {
            get
            {
                var options = ConfigurationOptions.Parse(ConnectionString);
                options.AbortOnConnectFail = false;
                return options;
            }
        }
        /// <summary>
        ///     获取 Redis 连接对象
        /// </summary>
        /// <returns></returns>
        public IConnectionMultiplexer GetConnectionRedisMultiplexer()
        {
            if(_connMultiplexer == null || !_connMultiplexer.IsConnected) {
                lock (Locker) {
                    if(_connMultiplexer == null || !_connMultiplexer.IsConnected) {
                        _connMultiplexer = ConnectionMultiplexer.Connect(ConnectionString);
                    }
                }
            }
            return _connMultiplexer;
        }
        public static RedisHelper InstanceByDb(int db)
        {
            return new RedisHelper(db);
        }
        #region 其它
        /// <summary>
        ///     分布式锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public void Lock(string key, Action action)
        {
            
            RedisValue token = Environment.MachineName;
            if(_db.LockTake(key, token, TimeSpan.FromSeconds(10))) {
                try {
                    action();
                }
                finally {
                    _db.LockRelease(key, token);
                }
            }
        }
        /// <summary>
        ///     分布式锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public T Lock<T>(string key, Func<T> action)
        {
            RedisValue token = Environment.MachineName;
            if(_db.LockTake(key, token, TimeSpan.FromSeconds(10))) {
                try {
                    return action();
                }
                finally {
                    _db.LockRelease(key, token);
                }
            }
            return default(T);
        }
        #endregion 其它
        #region 连接字符串
        /// <summary>
        ///     连接字符串
        /// </summary>
        private static readonly string ConnectionString = JsonConfigHelper.GetAppRedisStr;
        /// <summary>
        ///     默认的 Key 值（用来当作 RedisKey 的前缀）
        /// </summary>
        private static readonly string DefaultKey = "JYW";
        #endregion private field
        #region 构造函数
        static RedisHelper()
        {
            // AddRegisterEvent();
        }
        public RedisHelper(int db = 0)
        {
            _db = GetConnectionRedisMultiplexer().GetDatabase(db);
        }
        #endregion 构造函数
        #region String 操作
        /// <summary>
        ///     设置 key 并保存字符串（如果 key 已存在，则覆盖值）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool StringSet(string redisKey, string redisValue, TimeSpan? expiry = null)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.StringSet(redisKey, redisValue, expiry);
        }
        /// <summary>
        ///     保存多个 Key-value
        /// </summary>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public bool StringSet(IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        {
            var pairs = keyValuePairs.Select(x => new KeyValuePair<RedisKey, RedisValue>(AddKeyPrefix(x.Key), x.Value));
            return _db.StringSet(pairs.ToArray());
        }
        /// <summary>
        ///     获取字符串
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public string StringGet(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.StringGet(redisKey);
        }
        /// <summary>
        ///     存储一个对象（该对象会被序列化保存）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool StringSet<T>(string redisKey, T redisValue, TimeSpan? expiry = null)
        {
            redisKey = AddKeyPrefix(redisKey);
            var json = Serialize(redisValue);
            return _db.StringSet(redisKey, json, expiry);
        }
        /// <summary>
        ///     存储一个对象（该对象会被序列化保存）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="minutes">分钟</param>
        /// <returns></returns>
        public bool StringSet<T>(string redisKey, T redisValue, int minutes = 0)
        {
            redisKey = AddKeyPrefix(redisKey);
            var json = Serialize(redisValue);
            TimeSpan? time = null;
            if(minutes > 0) {
                time = new TimeSpan(0, 0, minutes, 0);
            }
            return _db.StringSet(redisKey, json, time);
        }
        /// <summary>
        ///     获取一个对象（会进行反序列化）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public T StringGet<T>(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return Deserialize<T>(_db.StringGet(redisKey));
        }
        #region async
        /// <summary>
        ///     保存一个字符串值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync(string redisKey, string redisValue, TimeSpan? expiry = null)
        {
            redisKey = AddKeyPrefix(redisKey);
            return await _db.StringSetAsync(redisKey, redisValue, expiry);
        }
        /// <summary>
        ///     保存一组字符串值
        /// </summary>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync(IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        {
            var pairs = keyValuePairs.Select(x => new KeyValuePair<RedisKey, RedisValue>(AddKeyPrefix(x.Key), x.Value));
            return await _db.StringSetAsync(pairs.ToArray());
        }
        /// <summary>
        ///     获取单个值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<string> StringGetAsync(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return await _db.StringGetAsync(redisKey);
        }
        /// <summary>
        ///     存储一个对象（该对象会被序列化保存）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync<T>(string redisKey, T redisValue, TimeSpan? expiry = null)
        {
            redisKey = AddKeyPrefix(redisKey);
            var json = Serialize(redisValue);
            return await _db.StringSetAsync(redisKey, json, expiry);
        }
        /// <summary>
        ///     存储一个对象（该对象会被序列化保存）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync<T>(string redisKey, T redisValue, int minutes = 0)
        {
            redisKey = AddKeyPrefix(redisKey);
            var json = Serialize(redisValue);
            TimeSpan? time = null;
            if(minutes > 0) {
                time = new TimeSpan(0, 0, minutes, 0);
            }
            return await _db.StringSetAsync(redisKey, json, time);
        }
        /// <summary>
        ///     获取一个对象（会进行反序列化）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<T> StringGetAsync<T>(string redisKey, TimeSpan? expiry = null)
        {
            redisKey = AddKeyPrefix(redisKey);
            return Deserialize<T>(await _db.StringGetAsync(redisKey));
        }
        #endregion async
        #endregion String 操作
        #region Hash 操作
        /// <summary>
        ///     判断该字段是否存在 hash 中
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public bool HashExists(string redisKey, string hashField)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.HashExists(redisKey, hashField);
        }
        /// <summary>
        ///     从 hash 中移除指定字段
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public bool HashDelete(string redisKey, string hashField)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.HashDelete(redisKey, hashField);
        }
        /// <summary>
        ///     从 hash 中移除指定字段
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashFields"></param>
        /// <returns></returns>
        public long HashDelete(string redisKey, IEnumerable<string> hashFields)
        {
            redisKey = AddKeyPrefix(redisKey);
            var fields = hashFields.Select(x => (RedisValue) x);
            return _db.HashDelete(redisKey, fields.ToArray());
        }
        /// <summary>
        ///     在 hash 设定值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public bool HashSet(string redisKey, string hashField, string value, int minutes = 0)
        {
            redisKey = AddKeyPrefix(redisKey);
            var result = _db.HashSet(redisKey, hashField, value);
            if(minutes > 0) {
                _db.KeyExpire(redisKey, new TimeSpan(0, 0, minutes));
            }
            return result;
        }
        /// <summary>
        ///     在 hash 中设定值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashFields"></param>
        /// <param name="minutes"></param>
        public void HashSet(string redisKey, IEnumerable<KeyValuePair<string, string>> hashFields, int minutes = 0)
        {
            redisKey = AddKeyPrefix(redisKey);
            var entries = hashFields.Select(x => new HashEntry(x.Key, x.Value));
            if(minutes > 0) {
                _db.KeyExpire(redisKey, new TimeSpan(0, 0, minutes, 0));
            }
            _db.HashSet(redisKey, entries.ToArray());
        }
        /// <summary>
        ///     在 hash 中获取值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public string HashGet(string redisKey, string hashField)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.HashGet(redisKey, hashField);
        }
        /// <summary>
        ///     在 hash 中获取值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashFields"></param>
        /// <returns></returns>
        public IEnumerable<string> HashGet(string redisKey, IEnumerable<string> hashFields)
        {
            redisKey = AddKeyPrefix(redisKey);
            var fields = hashFields.Select(x => (RedisValue) x);
            return ConvertStrings(_db.HashGet(redisKey, fields.ToArray()));
        }
        /// <summary>
        ///     从 hash 返回所有的字段值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public IEnumerable<string> HashKeys(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return ConvertStrings(_db.HashKeys(redisKey));
        }
        /// <summary>
        ///     返回 hash 中的所有值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public IEnumerable<string> HashValues(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return ConvertStrings(_db.HashValues(redisKey));
        }
        /// <summary>
        ///     在 hash 设定值（序列化）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="redisValue"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public bool HashSet<T>(string redisKey, string hashField, T redisValue, int minutes = 0)
        {
            redisKey = AddKeyPrefix(redisKey);
            var json = Serialize(redisValue);
            if(minutes > 0) {
                _db.KeyExpire(redisKey, new TimeSpan(0, 0, minutes, 0));
            }
            return _db.HashSet(redisKey, hashField, json);
        }
        /// <summary>
        ///     在 hash 中获取值（反序列化）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public T HashGet<T>(string redisKey, string hashField)
        {
            redisKey = AddKeyPrefix(redisKey);
            return Deserialize<T>(_db.HashGet(redisKey, hashField));
        }
        #region async
        /// <summary>
        ///     判断该字段是否存在 hash 中
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public async Task<bool> HashExistsAsync(string redisKey, string hashField)
        {
            redisKey = AddKeyPrefix(redisKey);
            return await _db.HashExistsAsync(redisKey, hashField);
        }
        /// <summary>
        ///     从 hash 中移除指定字段
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public async Task<bool> HashDeleteAsync(string redisKey, string hashField)
        {
            redisKey = AddKeyPrefix(redisKey);
            return await _db.HashDeleteAsync(redisKey, hashField);
        }
        /// <summary>
        ///     从 hash 中移除指定字段
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashFields"></param>
        /// <returns></returns>
        public async Task<long> HashDeleteAsync(string redisKey, IEnumerable<string> hashFields)
        {
            redisKey = AddKeyPrefix(redisKey);
            var fields = hashFields.Select(x => (RedisValue) x);
            return await _db.HashDeleteAsync(redisKey, fields.ToArray());
        }
        /// <summary>
        ///     在 hash 设定值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public async Task<bool> HashSetAsync(string redisKey, string hashField, string value, int minutes = 0)
        {
            redisKey = AddKeyPrefix(redisKey);
            var result = await _db.HashSetAsync(redisKey, hashField, value);
            if(minutes > 0) {
                _db.KeyExpire(redisKey, new TimeSpan(0, 0, minutes, 0));
            }
            return result;
        }
        /// <summary>
        ///     在 hash 中设定值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashFields"></param>
        public async Task HashSetAsync(string redisKey, IEnumerable<KeyValuePair<string, string>> hashFields)
        {
            redisKey = AddKeyPrefix(redisKey);
            var entries = hashFields.Select(x => new HashEntry(AddKeyPrefix(x.Key), x.Value));
            await _db.HashSetAsync(redisKey, entries.ToArray());
        }
        /// <summary>
        ///     在 hash 中获取值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public async Task<string> HashGetAsync(string redisKey, string hashField)
        {
            redisKey = AddKeyPrefix(redisKey);
            return await _db.HashGetAsync(redisKey, hashField);
        }
        /// <summary>
        ///     在 hash 中获取值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashFields"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> HashGetAsync(string redisKey, IEnumerable<string> hashFields, string value)
        {
            redisKey = AddKeyPrefix(redisKey);
            var fields = hashFields.Select(x => (RedisValue) x);
            return ConvertStrings(await _db.HashGetAsync(redisKey, fields.ToArray()));
        }
        /// <summary>
        ///     从 hash 返回所有的字段值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> HashKeysAsync(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return ConvertStrings(await _db.HashKeysAsync(redisKey));
        }
        /// <summary>
        ///     返回 hash 中的所有值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> HashValuesAsync(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return ConvertStrings(await _db.HashValuesAsync(redisKey));
        }
        /// <summary>
        ///     在 hash 设定值（序列化）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public async Task<bool> HashSetAsync<T>(string redisKey, string hashField, T value, int minutes = 0)
        {
            redisKey = AddKeyPrefix(redisKey);
            var json = Serialize(value);
            var result = await _db.HashSetAsync(redisKey, hashField, Serialize(value));
            if(minutes > 0) {
                _db.KeyExpire(redisKey, new TimeSpan(0, 0, minutes, 0));
            }
            return result;
        }
        /// <summary>
        ///     在 hash 中获取值（反序列化）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public async Task<T> HashGetAsync<T>(string redisKey, string hashField)
        {
            redisKey = AddKeyPrefix(redisKey);
            return Deserialize<T>(await _db.HashGetAsync(redisKey, hashField));
        }
        #endregion async
        #endregion Hash 操作
        #region List 操作
        /// <summary>
        ///     移除并返回存储在该键列表的第一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public string ListLeftPop(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.ListLeftPop(redisKey);
        }
        /// <summary>
        ///     移除并返回存储在该键列表的最后一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public string ListRightPop(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.ListRightPop(redisKey);
        }
        /// <summary>
        ///     移除列表指定键上与该值相同的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public long ListRemove(string redisKey, string redisValue)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.ListRemove(redisKey, redisValue);
        }
        /// <summary>
        ///     在列表尾部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public long ListRightPush(string redisKey, string redisValue)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.ListRightPush(redisKey, redisValue);
        }
        /// <summary>
        ///     在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public long ListLeftPush(string redisKey, string redisValue)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.ListLeftPush(redisKey, redisValue);
        }
        /// <summary>
        ///     返回列表上该键的长度，如果不存在，返回 0
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public long ListLength(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.ListLength(redisKey);
        }
        /// <summary>
        ///     返回在该列表上键所对应的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public IEnumerable<string> ListRange(string redisKey, long start = 0L, long stop = -1L)
        {
            redisKey = AddKeyPrefix(redisKey);
            return ConvertStrings(_db.ListRange(redisKey, start, stop));
        }
        /// <summary>
        ///     返回在该列表上键所对应的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public List<T> ListRange<T>(string redisKey, long start = 0L, long stop = -1L)
        {
            redisKey = AddKeyPrefix(redisKey);
            var list = _db.ListRange(redisKey, start, stop);
            var enumerable = new List<T>();
            foreach (var m in list) {
                enumerable.Add(Deserialize<T>(m));
            }
            return enumerable;
        }
        /// <summary>
        ///     移除并返回存储在该键列表的第一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public T ListLeftPop<T>(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return Deserialize<T>(_db.ListLeftPop(redisKey));
        }
        /// <summary>
        ///     移除并返回存储在该键列表的最后一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public T ListRightPop<T>(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return Deserialize<T>(_db.ListRightPop(redisKey));
        }
        /// <summary>
        ///     在列表尾部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public long ListRightPush<T>(string redisKey, T redisValue)
        {
            redisKey = AddKeyPrefix(redisKey);
            
            return _db.ListRightPush(redisKey, Serialize(redisValue));
        }
        /// <summary>
        ///     在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public long ListLeftPush<T>(string redisKey, T redisValue)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.ListLeftPush(redisKey, Serialize(redisValue));
        }
        #region List-async
        /// <summary>
        ///     移除并返回存储在该键列表的第一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<string> ListLeftPopAsync(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return await _db.ListLeftPopAsync(redisKey);
        }
        /// <summary>
        ///     移除并返回存储在该键列表的最后一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<string> ListRightPopAsync(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return await _db.ListRightPopAsync(redisKey);
        }
        /// <summary>
        ///     移除列表指定键上与该值相同的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListRemoveAsync(string redisKey, string redisValue)
        {
            redisKey = AddKeyPrefix(redisKey);
            return await _db.ListRemoveAsync(redisKey, redisValue);
        }
        /// <summary>
        ///     在列表尾部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListRightPushAsync(string redisKey, string redisValue)
        {
            redisKey = AddKeyPrefix(redisKey);
            return await _db.ListRightPushAsync(redisKey, redisValue);
        }
        /// <summary>
        ///     在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListLeftPushAsync(string redisKey, string redisValue)
        {
            redisKey = AddKeyPrefix(redisKey);
            return await _db.ListLeftPushAsync(redisKey, redisValue);
        }
        /// <summary>
        ///     返回列表上该键的长度，如果不存在，返回 0
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<long> ListLengthAsync(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return await _db.ListLengthAsync(redisKey);
        }
        /// <summary>
        ///     返回在该列表上键所对应的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> ListRangeAsync(string redisKey, long start = 0L, long stop = -1L)
        {
            redisKey = AddKeyPrefix(redisKey);
            var query = await _db.ListRangeAsync(redisKey, start, stop);
            return query.Select(x => x.ToString());
        }
        /// <summary>
        ///     移除并返回存储在该键列表的第一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<T> ListLeftPopAsync<T>(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return Deserialize<T>(await _db.ListLeftPopAsync(redisKey));
        }
        /// <summary>
        ///     移除并返回存储在该键列表的最后一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<T> ListRightPopAsync<T>(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return Deserialize<T>(await _db.ListRightPopAsync(redisKey));
        }
        /// <summary>
        ///     在列表尾部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListRightPushAsync<T>(string redisKey, T redisValue)
        {
            redisKey = AddKeyPrefix(redisKey);
            return await _db.ListRightPushAsync(redisKey, Serialize(redisValue));
        }
        /// <summary>
        ///     在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListLeftPushAsync<T>(string redisKey, T redisValue)
        {
            redisKey = AddKeyPrefix(redisKey);
            return await _db.ListLeftPushAsync(redisKey, Serialize(redisValue));
        }
        #endregion List-async
        #endregion List 操作
        #region SortedSet 操作
        /// <summary>
        ///     SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public bool SortedSetAdd(string redisKey, string member, double score)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.SortedSetAdd(redisKey, member, score);
        }
        /// <summary>
        ///     在有序集合中返回指定范围的元素，默认情况下从低到高。
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public IEnumerable<string> SortedSetRangeByRank(string redisKey, long start = 0L, long stop = -1L)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.SortedSetRangeByRank(redisKey, start, stop).Select(x => x.ToString());
        }
        /// <summary>
        ///     返回有序集合的元素个数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public long SortedSetLength(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.SortedSetLength(redisKey);
        }
        /// <summary>
        ///     返回有序集合的元素个数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="memebr"></param>
        /// <returns></returns>
        public bool SortedSetLength(string redisKey, string memebr)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.SortedSetRemove(redisKey, memebr);
        }
        /// <summary>
        ///     SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public bool SortedSetAdd<T>(string redisKey, T member, double score)
        {
            redisKey = AddKeyPrefix(redisKey);
            var json = Serialize(member);
            return _db.SortedSetAdd(redisKey, json, score);
        }
        /// <summary>
        ///     增量的得分排序的集合中的成员存储键值键按增量
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public double SortedSetIncrement(string redisKey, string member, double value = 1)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.SortedSetIncrement(redisKey, member, value);
        }
        #region SortedSet-Async
        /// <summary>
        ///     SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public async Task<bool> SortedSetAddAsync(string redisKey, string member, double score)
        {
            redisKey = AddKeyPrefix(redisKey);
            return await _db.SortedSetAddAsync(redisKey, member, score);
        }
        /// <summary>
        ///     在有序集合中返回指定范围的元素，默认情况下从低到高。
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> SortedSetRangeByRankAsync(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return ConvertStrings(await _db.SortedSetRangeByRankAsync(redisKey));
        }
        /// <summary>
        ///     返回有序集合的元素个数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<long> SortedSetLengthAsync(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return await _db.SortedSetLengthAsync(redisKey);
        }
        /// <summary>
        ///     返回有序集合的元素个数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="memebr"></param>
        /// <returns></returns>
        public async Task<bool> SortedSetRemoveAsync(string redisKey, string memebr)
        {
            redisKey = AddKeyPrefix(redisKey);
            return await _db.SortedSetRemoveAsync(redisKey, memebr);
        }
        /// <summary>
        ///     SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public async Task<bool> SortedSetAddAsync<T>(string redisKey, T member, double score)
        {
            redisKey = AddKeyPrefix(redisKey);
            var json = Serialize(member);
            return await _db.SortedSetAddAsync(redisKey, json, score);
        }
        /// <summary>
        ///     增量的得分排序的集合中的成员存储键值键按增量
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task<double> SortedSetIncrementAsync(string redisKey, string member, double value = 1)
        {
            redisKey = AddKeyPrefix(redisKey);
            return _db.SortedSetIncrementAsync(redisKey, member, value);
        }
        #endregion SortedSet-Async
        #endregion SortedSet 操作
        #region key 操作
        /// <summary>
        ///     移除指定 Key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public bool KeyDelete(string redisKey)
        {
            try {
                redisKey = AddKeyPrefix(redisKey);
                return _db.KeyDelete(redisKey);
            }
            catch (Exception) {
            }
            return false;
        }
        /// <summary>
        ///     移除指定 Key
        /// </summary>
        /// <param name="redisKeys"></param>
        /// <returns></returns>
        public long KeyDelete(IEnumerable<string> redisKeys)
        {
            var keys = redisKeys.Select(x => (RedisKey) AddKeyPrefix(x));
            return _db.KeyDelete(keys.ToArray());
        }
        /// <summary>
        ///     校验 Key 是否存在
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public bool KeyExists(string redisKey)
        {
            try {
                redisKey = AddKeyPrefix(redisKey);
                return _db.KeyExists(redisKey);
            }
            catch (Exception) {
            }
            return false;
        }
        /// <summary>
        ///     重命名 Key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisNewKey"></param>
        /// <returns></returns>
        public bool KeyRename(string redisKey, string redisNewKey)
        {
            try {
                redisKey = AddKeyPrefix(redisKey);
                return _db.KeyRename(redisKey, redisNewKey);
            }
            catch (Exception) {
            }
            return false;
        }
        /// <summary>
        ///     设置 Key 的时间
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool KeyExpire(string redisKey, TimeSpan? expiry)
        {
            try {
                redisKey = AddKeyPrefix(redisKey);
                return _db.KeyExpire(redisKey, expiry);
            }
            catch (Exception) {
            }
            return false;
        }
        #region key-async
        /// <summary>
        ///     移除指定 Key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<bool> KeyDeleteAsync(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return await _db.KeyDeleteAsync(redisKey);
        }
        /// <summary>
        ///     移除指定 Key
        /// </summary>
        /// <param name="redisKeys"></param>
        /// <returns></returns>
        public async Task<long> KeyDeleteAsync(IEnumerable<string> redisKeys)
        {
            var keys = redisKeys.Select(x => (RedisKey) AddKeyPrefix(x));
            return await _db.KeyDeleteAsync(keys.ToArray());
        }
        /// <summary>
        ///     校验 Key 是否存在
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<bool> KeyExistsAsync(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return await _db.KeyExistsAsync(redisKey);
        }
        /// <summary>
        ///     重命名 Key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisNewKey"></param>
        /// <returns></returns>
        public async Task<bool> KeyRenameAsync(string redisKey, string redisNewKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return await _db.KeyRenameAsync(redisKey, redisNewKey);
        }
        /// <summary>
        ///     设置 Key 的时间
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<bool> KeyExpireAsync(string redisKey, TimeSpan? expiry)
        {
            redisKey = AddKeyPrefix(redisKey);
            return await _db.KeyExpireAsync(redisKey, expiry);
        }
        #endregion key-async
        #endregion key 操作
        #region 发布订阅
        /// <summary>
        ///     订阅
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="handle"></param>
        public void Subscribe(RedisChannel channel, Action<RedisChannel, RedisValue> handle)
        {
            using (var conn = ConnectionMultiplexer.Connect(Options)) {
                var sub = conn.GetSubscriber();
                sub.Subscribe(channel, handle);
            }
        }
        /// <summary>
        ///     发布
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public long Publish(RedisChannel channel, RedisValue message)
        {
            using (var conn = ConnectionMultiplexer.Connect(Options)) {
                var sub = conn.GetSubscriber();
                return sub.Publish(channel, message);
            }
        }
        /// <summary>
        ///     发布（使用序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public long Publish<T>(RedisChannel channel, T message)
        {
            using (var conn = ConnectionMultiplexer.Connect(Options)) {
                var sub = conn.GetSubscriber();
                return sub.Publish(channel, Serialize(message));
            }
        }
        #region 发布订阅-async
        /// <summary>
        ///     订阅
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="handle"></param>
        public async Task SubscribeAsync(RedisChannel channel, Action<RedisChannel, RedisValue> handle)
        {
            using (var conn = ConnectionMultiplexer.Connect(Options)) {
                var sub = conn.GetSubscriber();
                await sub.SubscribeAsync(channel, handle);
            }
        }
        /// <summary>
        ///     发布
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<long> PublishAsync(RedisChannel channel, RedisValue message)
        {
            using (var conn = ConnectionMultiplexer.Connect(Options)) {
                var sub = conn.GetSubscriber();
                return await sub.PublishAsync(channel, message);
            }
        }
        /// <summary>
        ///     发布（使用序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<long> PublishAsync<T>(RedisChannel channel, T message)
        {
            using (var conn = ConnectionMultiplexer.Connect(Options)) {
                var sub = conn.GetSubscriber();
                return await sub.PublishAsync(channel, Serialize(message));
            }
        }
        #endregion 发布订阅-async
        #endregion 发布订阅
        #region private method
        /// <summary>
        ///     添加 Key 的前缀
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string AddKeyPrefix(string key)
        {
            return $"{DefaultKey}:{key}";
        }
        /// <summary>
        ///     转换为字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        private static IEnumerable<string> ConvertStrings<T>(IEnumerable<T> list) where T : struct
        {
            if(list == null) {
                throw new ArgumentNullException(nameof(list));
            }
            return list.Select(x => x.ToString());
        }
        /// <summary>
        ///     序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static byte[] Serialize(object obj)
        {
            if(obj == null) {
                return null;
            }
            try {
                var binaryFormatter = new BinaryFormatter();
                using (var memoryStream = new MemoryStream()) {
                    binaryFormatter.Serialize(memoryStream, obj);
                    var data = memoryStream.ToArray();
                    return data;
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }
        /// <summary>
        ///     反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private static T Deserialize<T>(byte[] data)
        {
            if(data == null) {
                return default(T);
            }
            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream(data)) {
                var result = (T) binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }
        public void KeyDelete(string saleKey, string v)
        {
            throw new NotImplementedException();
        }
        #endregion private method
    }
}