using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XDF.Core.Helper.Redis;
using XDF.Core.Model;
using XDF.Data;

namespace XDF.Service
{
    public class BaseService<T> where T : class
    {
        public static readonly string Key = typeof(T).Name;
        public BaseDao<T> BaseCrudDao = null;
        #region CRUD
        public virtual T Find(object id)
        {
            return BaseCrudDao.Find(id);
        }
        public virtual T FindByCode(object code, bool isUseCache = true)
        {
            T entity = null;
            if (!isUseCache)
            {
                return BaseCrudDao.Find("Code=@Code", new { code });
            }
            var key = Key + "_Code";
            entity = RedisHelper.InstanceRedis.HashGet<T>(key, code.ToString());
            if (entity == null)
            {
                entity = Find("Code=@Code", new { code });
                if (entity != null)
                {
                    RedisHelper.InstanceRedis.HashSet(key, code.ToString(), entity);
                }
            }
            return entity;
        }
        public virtual T Find(string where, object par)
        {
            return BaseCrudDao.Find(where, par, "*");
        }
        public virtual T Find(string where, object par, string field)
        {
            return BaseCrudDao.Find(where, par, field);
        }
        public virtual List<T> Filter()
        {
            return BaseCrudDao.Filter();
        }
        public virtual PageListModel<T> Filter(PageListModel<T> info)
        {
            return BaseCrudDao.Filter(info);
        }
        public virtual List<T> Filter(int top, string where, object par, string order = "", string field = "*")
        {
            return BaseCrudDao.Filter(top, where, par, order, field);
        }
        public virtual List<T> Filter(string where, object par)
        {
            return BaseCrudDao.Filter(where, par, field: "*");
        }
        public virtual List<T> Filter(string where, object par, string order, string field = "*")
        {
            return BaseCrudDao.Filter(where, par, order, field);
        }

        public virtual int Del(object id)
        {
            return BaseCrudDao.Del(id);
        }
        public virtual int Del(string where, object par)
        {
            return BaseCrudDao.Del(where, par);
        }
        public virtual int Insert(T info)
        {
            return BaseCrudDao.Insert(info);
        }
        public virtual int Edit(T info)
        {
            return BaseCrudDao.Edit(info);
        }
        public int Edit(string where, string val, object par)
        {
            return BaseCrudDao.Edit(where, val, par);
        }
        public virtual int Count(string where, object par)
        {
            return BaseCrudDao.Count(where, par);
        }
        /// <summary>
        ///     返回查询第一行第一列
        /// </summary>
        /// <param name="where"></param>
        /// <param name="par"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public virtual TKey Single<TKey>(string where, object par, string field)
        {
            return BaseCrudDao.Single<TKey>(where, par, field);
        }
        #endregion
        #region async
        public virtual Task<T> FindAsync(object id)
        {
            return BaseCrudDao.FindAsync(id);
        }
        public virtual Task<T> FindAsync(string where, object par, string field = "*")
        {
            return BaseCrudDao.FindAsync(where, par, field);
        }
        public virtual System.Threading.Tasks.Task<IEnumerable<T>> FilterAsync()
        {
            return BaseCrudDao.FilterAsync();
        }
        public virtual Task<IEnumerable<T>> FilterAsync(int top, string where, object par, string order = "", string field = "*")
        {
            return BaseCrudDao.FilterAsync(top, where, par, order, field);
        }
        public virtual Task<IEnumerable<T>> FilterAsync(string where, object par, string order = "", string field = "*")
        {
            return BaseCrudDao.FilterAsync(where, par, order, field);
        }
        public virtual Task<int> DelAsync(object id)
        {
            return BaseCrudDao.DelAsync(id);
        }
        public virtual Task<int> DelAsync(string where, object par)
        {
            return BaseCrudDao.DelAsync(where, par);
        }
        public virtual async Task<TType> InsertAsync<TType>(T info)
        {
            return await BaseCrudDao.InsertAsync<TType>(info);
        }
        public virtual async Task<int> EditAsync(T info)
        {
            return await BaseCrudDao.EditAsync(info);
        }
        public virtual async Task<int> EditAsync(string val, object par)
        {
            return await BaseCrudDao.EditAsync(val, par);
        }
        public virtual async Task<int> EditAsync(string where, string val, object par)
        {
            return await BaseCrudDao.EditAsync(where, val, par);
        }
        #endregion
        #region  cache

        public virtual int Edit(T info, object id)
        {
            RemoveCache(id);
            var ids = BaseCrudDao.Edit(info);
            RemoveCache(id);
            return ids;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="par"></param>
        /// <param name="id">移除缓存ID</param>
        /// <returns></returns>
        public virtual int Edit(string val, object par, object id)
        {
            RemoveCache(id);
            var ids = BaseCrudDao.Edit(val, par);
            RemoveCache(id);
            return ids;
        }
        public virtual T FindByCache(object id)
        {
            var key = Key + "_Id";
            var entity = RedisHelper.InstanceRedis.HashGet<T>(key, id.ToString());
            if (entity == null)
            {
                entity = Find(id);
                if (entity != null)
                {
                    RedisHelper.InstanceRedis.HashSet(key, id.ToString(), entity);
                }
            }
            return entity;
        }
        public virtual List<T> FilterByCache()
        {
            var entity = RedisHelper.InstanceRedis.StringGet<List<T>>(Key + "_List");
            if (entity != null && entity.Any()) return entity;
            entity = Filter();
            if (entity != null)
            {
                RedisHelper.InstanceRedis.StringSet(Key + "_List", entity, 60);
            }
            return entity;
        }
        public virtual void RemoveCache(object id)
        {
            var key = Key + "_Id";
            RedisHelper.InstanceRedis.HashDelete(Key + "_Code", id.ToString());
            RedisHelper.InstanceRedis.HashDelete(key, id.ToString());
            RedisHelper.InstanceRedis.KeyDelete(Key + "_List");
        }
        public virtual void RemoveCachByList()
        {
            RedisHelper.InstanceRedis.KeyDelete(Key);
        }
        #endregion
    }
}
