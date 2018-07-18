﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using XDF.Core.Helper.Db;
using XDF.Core.Helper.JsonConfig;
using XDF.Core.Helper.Mongo.Base;
using XDF.Core.Model;

namespace XDF.Data
{
    public class BaseDao<T> : IDisposable  where T:class 
    {

        public readonly DapperHelper dapperHelper;
        ///主建是否自增，如果不自增插入的时候要赋值
        public string _Field = "";
        public string _FieldAt = "";
        public string _FieldUp = "";
        public bool _IsIdentity = true;
        public string _Primary = "";
        public string _Table = "";
        public BaseDao(string dbName)
        {
            dapperHelper = new DapperHelper(dbName);
        }
        #region CRUD
        public T Find(object id)
        {
            string sql = string.Format(dapperHelper.ConnectionConfig.Type==DbType.sqlserver ? SqlServerFormat.Find : MySqlFormat.Find, _Primary + "," + _Field, _Table, $" WHERE {_Primary}=@{_Primary}");
            var par = new DynamicParameters();
            par.Add("@" + _Primary, id);
            return dapperHelper.Find<T>(sql, par);
        }
        public T Find(string where, object par, string field = "*")
        {
            var sql = new StringBuilder();
            sql.Append("SELECT ");
            if (dapperHelper.ConnectionConfig.Type==DbType.sqlserver)
            {
                sql.Append(" TOP 1 ");
            }
            if (field.IsStringEmpty() || field == "*")
            {
                sql.AppendFormat("{0},{1}", _Primary, _Field);
            }
            else
            {
                sql.Append(field);
            }
            sql.AppendFormat(" FROM {0} ", _Table);
            if (!where.IsStringEmpty())
            {
                sql.AppendFormat(" WHERE {0} ", where);
            }
            if (dapperHelper.ConnectionConfig.Type == DbType.mysql)
            {
                sql.Append(" limit 1 ");
            }
            return dapperHelper.Find<T>(sql.ToString(), par);
        }
        public List<T> Filter()
        {
            var sql=string.Format(dapperHelper.ConnectionConfig.Type==DbType.sqlserver?SqlServerFormat.Filter:MySqlFormat.Filter, _Primary, _Field, _Table);
            return dapperHelper.Filter<T>(sql.ToString(), null);
        }
        public PageListModel<T> Filter<T>(PageListModel<T> info)
        {
            if (info.Primary.IsStringEmpty())
            {
                info.Primary = _Primary;
            }
            if (info.Field.IsStringEmpty())
            {
                info.Field = string.Format("{0},{1}", _Primary, _Field);
            }
            if (info.Table.IsStringEmpty())
            {
                info.Table = _Table;
            }
            return dapperHelper.Filter(info);
        }
        public List<T> Filter(int top, string where, object par, string order = "", string field = "*")
        {
            var sql = new StringBuilder();
            sql.Append("SELECT TOP ");
            sql.Append(top);
            sql.Append(" ");
            if (field.IsStringEmpty() || field == "*")
            {
                sql.AppendFormat("{0},{1}", _Primary, _Field);
            }
            else
            {
                sql.Append(field);
            }
            sql.AppendFormat(" FROM {0}", _Table);
            if (!where.IsStringEmpty())
            {
                sql.AppendFormat(" WHERE {0}", where);
            }
            if (!order.IsStringEmpty())
            {
                sql.AppendFormat(" ORDER BY {0}", order);
            }
            else
            {
                sql.AppendFormat(" ORDER BY {0} DESC", _Primary);
            }
            return dapperHelper.Filter<T>(sql.ToString(), par);
        }
        public List<T> Filter(string where, object par, string order = "", string field = "*")
        {
            var sql = new StringBuilder();
            sql.Append("SELECT ");
            if (field.IsStringEmpty() || field == "*")
            {
                sql.AppendFormat("{0},{1}", _Primary, _Field);
            }
            else
            {
                sql.Append(field);
            }
            sql.AppendFormat(" FROM {0}", _Table);
            if (!where.IsStringEmpty())
            {
                sql.AppendFormat(" WHERE {0}", where);
            }
            if (!order.IsStringEmpty())
            {
                sql.AppendFormat(" ORDER BY {0}", order);
            }
            else
            {
                sql.AppendFormat(" ORDER BY {0} DESC", _Primary);
            }
            return dapperHelper.Filter<T>(sql.ToString(), par);
        }

        public int Del(object id)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(SqlServerFormat.Del, _Table);
            sql.AppendFormat(" WHERE {0}=@{0}", _Primary);
            var par = new DynamicParameters();
            par.Add("@" + _Primary, id);
            return dapperHelper.Execute(sql.ToString(), par);
        }
        public int Del(string where, object par)
        {
            if (where.Trim().IsStringEmpty())
            {
                return 0;
            }
            var sql = new StringBuilder();
            sql.AppendFormat(SqlServerFormat.Del, _Table);
            if (!where.IsStringEmpty())
            {
                sql.AppendFormat(" WHERE {0} ", where);
            }
            return dapperHelper.Execute(sql.ToString(), par);
        }
        public T Insert<T>(T info)
        {
            var regex = new Regex(@"(?<=\])\.\w+\(\)\w+(?=\,)");
            var strSql = new StringBuilder();
            if (_IsIdentity)
            {
                strSql.AppendFormat(SqlServerFormat.Insert, _Table, regex.Replace(_Field, ""), _FieldAt);
            }
            else
            {
                strSql.AppendFormat(SqlServerFormat.Insert, _Table, _Primary + "," + regex.Replace(_Field, ""), "@" + _Primary + "," + _FieldAt);
            }
            return dapperHelper.Single<T>(strSql.ToString(), info);
        }
        public int Add(T info)
        {
            var regex = new Regex(@"(?<=\])\.\w+\(\)\w+(?=\,)");
            var strSql = new StringBuilder();
            if (_IsIdentity)
            {
                strSql.AppendFormat(SqlServerFormat.Insert, _Table, regex.Replace(_Field, ""), _FieldAt);
            }
            else
            {
                strSql.AppendFormat(SqlServerFormat.Insert, _Table, _Primary + "," + regex.Replace(_Field, ""), "@" + _Primary + "," + _FieldAt);
            }
            return dapperHelper.Execute(strSql.ToString(), info);
        }
        public int Edit(T info)
        {
            var strSql = new StringBuilder();
            strSql.AppendFormat(SqlServerFormat.Update, _Table, _FieldUp);
            strSql.AppendFormat(" WHERE {0}=@{0}", _Primary);
            return dapperHelper.Execute(strSql.ToString(), info);
        }
        public int Edit(string val, object par)
        {
            var sql = new StringBuilder();
            sql.AppendFormat("UPDATE  {0} {1}", _Table, val);
            return dapperHelper.Execute(sql.ToString(), par);
        }
        public int Edit(string where, string val, object par)
        {
            var sql = new StringBuilder();
            sql.AppendFormat("UPDATE  {0} {1}", _Table, val);
            if (!where.IsStringEmpty())
            {
                sql.AppendFormat(" WHERE {0} ", where);
            }
            return dapperHelper.Execute(sql.ToString(), par);
        }
        public T Count<T>(string where, object par)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(SqlServerFormat.Count, _Table);
            if (!where.IsStringEmpty())
            {
                sql.AppendFormat(" WHERE {0} ", where);
            }
            return dapperHelper.Single<T>(sql.ToString(), par);
        }
        public virtual T Single<T>(string where, object par, string field)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(SqlServerFormat.Single, field, _Table);
            if (!where.IsStringEmpty())
            {
                sql.AppendFormat(" WHERE {0}", where);
            }
            return dapperHelper.Single<T>(sql.ToString(), par);
        }
        public void Dispose()
        {
        }
        #endregion
        #region async
        public async Task<T> FindAsync(object id)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(SqlServerFormat.Find, _Primary + "," + _Field, _Table, $" WHERE {_Primary}=@{_Primary}");
            var par = new DynamicParameters();
            par.Add("@" + _Primary, id);
            return await dapperHelper.FindAsync<T>(sql.ToString(), par);
        }
        public async Task<T> FindAsync(string where, object par, string field = "*")
        {
            var sql = new StringBuilder();
            sql.Append("SELECT ");
            if (field.IsStringEmpty() || field == "*")
            {
                sql.AppendFormat("{0},{1}", _Primary, _Field);
            }
            else
            {
                sql.Append(field);
            }
            sql.AppendFormat(" FROM {0} ", _Table);
            if (!where.IsStringEmpty())
            {
                sql.AppendFormat(" WHERE {0}", where);
            }
            return await dapperHelper.FindAsync<T>(sql.ToString(), par);
        }
        public Task<IEnumerable<T>> FilterAsync()
        {
            var sql = new StringBuilder();
            sql.AppendFormat(SqlServerFormat.Filter, _Primary, _Field, _Table);
            return dapperHelper.FilterAsync<T>(sql.ToString(), null);
        }
        public async Task<IEnumerable<T>> FilterAsync(int top, string where, object par, string order = "", string field = "*")
        {
            var sql = new StringBuilder();
            sql.Append("SELECT TOP ");
            sql.Append(top);
            sql.Append(" ");
            if (field.IsStringEmpty() || field == "*")
            {
                sql.AppendFormat("{0},{1}", _Primary, _Field);
            }
            else
            {
                sql.Append(field);
            }
            sql.AppendFormat(" FROM {0}", _Table);
            if (!where.IsStringEmpty())
            {
                sql.AppendFormat(" WHERE {0}", where);
            }
            if (!order.IsStringEmpty())
            {
                sql.AppendFormat(" ORDER BY {0}", order);
            }
            else
            {
                sql.AppendFormat(" ORDER BY {0} DESC", _Primary);
            }
            return await dapperHelper.FilterAsync<T>(sql.ToString(), par);
        }
        public async Task<IEnumerable<T>> FilterAsync(string where, object par, string order = "", string field = "*")
        {
            var sql = new StringBuilder();
            sql.Append("SELECT ");
            if (field.IsStringEmpty() || field == "*")
            {
                sql.AppendFormat("{0},{1}", _Primary, _Field);
            }
            else
            {
                sql.Append(field);
            }
            sql.AppendFormat(" FROM {0}", _Table);
            if (!where.IsStringEmpty())
            {
                sql.AppendFormat(" WHERE {0}", where);
            }
            if (!order.IsStringEmpty())
            {
                sql.AppendFormat(" ORDER BY {0}", order);
            }
            else
            {
                sql.AppendFormat(" ORDER BY {0} DESC", _Primary);
            }
            return await dapperHelper.FilterAsync<T>(sql.ToString(), par);
        }
        public async Task<int> DelAsync(object id)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(SqlServerFormat.Del, _Table);
            sql.AppendFormat(" WHERE {0}=@{0}", _Primary);
            var par = new DynamicParameters();
            par.Add("@" + _Primary, id);
            return await dapperHelper.ExecuteAsync(sql.ToString(), par);
        }
        public async Task<int> DelAsync(string where, object par)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(SqlServerFormat.Del, _Table);
            if (!where.IsStringEmpty())
            {
                sql.AppendFormat(" WHERE {0} ", where);
            }
            return await dapperHelper.ExecuteAsync(sql.ToString(), par);
        }
        public async Task<M> InsertAsync<M>(T info)
        {
            var regex = new Regex(@"(?<=\])\.\w+\(\)\w+(?=\,)");
            var strSql = new StringBuilder();
            if (_IsIdentity)
            {
                strSql.AppendFormat(SqlServerFormat.Insert, _Table, regex.Replace(_Field, ""), _FieldAt);
            }
            else
            {
                strSql.AppendFormat(SqlServerFormat.Insert, _Table, _Primary + "," + regex.Replace(_Field, ""), "@" + _Primary + "," + _FieldAt);
            }
            return await dapperHelper.SingleAsync<M>(strSql.ToString(), info);
        }
        public async Task<int> EditAsync(T info)
        {
            var strSql = new StringBuilder();
            strSql.AppendFormat(SqlServerFormat.Update, _Table, _FieldUp);
            strSql.AppendFormat(" WHERE {0}=@{0}", _Primary);
            return await dapperHelper.ExecuteAsync(strSql.ToString(), info);
        }
        public async Task<int> EditAsync(string val, object par)
        {
            var sql = new StringBuilder();
            sql.AppendFormat("UPDATE  {0} {1}", _Table, val);
            return await dapperHelper.ExecuteAsync(sql.ToString(), par);
        }
        public async Task<int> EditAsync(string where, string val, object par)
        {
            var sql = new StringBuilder();
            sql.AppendFormat("UPDATE  {0} {1}", _Table, val);
            if (!where.IsStringEmpty())
            {
                sql.AppendFormat(" WHERE {0} ", where);
            }
            return await dapperHelper.ExecuteAsync(sql.ToString(), par);
        }
        #endregion
    }
}
