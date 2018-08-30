using System;
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
            var sql = "";
            if (dapperHelper.ConnectionConfig.Type == DbType.sqlserver)
            {
                sql = $"SELECT TOP 1  {_Primary},{_Field} FROM {_Table} WHERE {_Primary}=@{_Primary}";
            }

            if (dapperHelper.ConnectionConfig.Type==DbType.mysql)
            {
                sql = $"SELECT  {_Primary},{_Field} FROM {_Table} WHERE {_Primary}=@{_Primary} LIMIT 1";
            }
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
            var sql= $"SELECT {_Primary},{_Field} FROM {_Table}";
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
            sql.Append("SELECT ");
            if (dapperHelper.ConnectionConfig.Type==DbType.sqlserver)
            {
                sql.Append($" TOP {top}");
            }
            if (field.IsStringEmpty() || field == "*")
            {
                sql.AppendFormat(" {0},{1} ", _Primary, _Field);
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

            if (dapperHelper.ConnectionConfig.Type == DbType.mysql)
            {
                sql.Append($" Limit {top}");
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
            sql.AppendFormat($"DELETE FROM {_Table}");
            sql.AppendFormat($" WHERE {_Primary}=@{_Primary}");
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
            sql.AppendFormat("DELETE FROM {0}", _Table);
            if (!where.IsStringEmpty())
            {
                sql.AppendFormat(" WHERE {0} ", where);
            }
            return dapperHelper.Execute(sql.ToString(), par);
        }
        public int Insert(T info)
        {
            var regex = new Regex(@"(?<=\])\.\w+\(\)\w+(?=\,)");
            var strSql = new StringBuilder();
            if (_IsIdentity)
            {
                strSql.AppendFormat("INSERT INTO {0}({1}) VALUES ({2})", _Table, regex.Replace(_Field, ""), _FieldAt);
            }
            else
            {
                strSql.AppendFormat("INSERT INTO {0}({1}) VALUES ({2})", _Table, _Primary + "," + regex.Replace(_Field, ""), "@" + _Primary + "," + _FieldAt);
            }
            return dapperHelper.Execute(strSql.ToString(), info);
        }
        public int Edit(T info)
        {
            var strSql = new StringBuilder();
            strSql.AppendFormat("UPDATE {0} SET {1}", _Table, _FieldUp);
            strSql.AppendFormat(" WHERE {0}=@{0}", _Primary);
            return dapperHelper.Execute(strSql.ToString(), info);
        }
        public int Edit(string val, object par)
        {
            var sql = new StringBuilder();
            sql.AppendFormat("UPDATE   {0} SET {1}", _Table, val);
            return dapperHelper.Execute(sql.ToString(), par);
        }
        public int Edit(string where, string val, object par)
        {
            var sql = new StringBuilder();
            sql.AppendFormat("UPDATE  {0} SET {1}", _Table, val);
            if (!where.IsStringEmpty())
            {
                sql.AppendFormat(" WHERE {0} ", where);
            }
            return dapperHelper.Execute(sql.ToString(), par);
        }
        public int Count(string where, object par)
        {
            var sql = new StringBuilder();
            sql.Append($"SELECT COUNT(1) FROM {_Table}");
            if (!where.IsStringEmpty())
            {
                sql.Append($" WHERE {where}");
            }
            return dapperHelper.Single<int>(sql.ToString(), par);
        }
        public virtual T Single<T>(string where, object par, string field)
        {
            var sql = new StringBuilder();
            sql.Append($"SELECT {field} FROM {_Table}");
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
            var sql = "";
            if (dapperHelper.ConnectionConfig.Type == DbType.sqlserver)
            {
                sql = $"SELECT TOP 1  {_Primary},{_Field} FROM {_Table} WHERE {_Primary}=@{_Primary}";
            }

            if (dapperHelper.ConnectionConfig.Type == DbType.mysql)
            {
                sql = $"SELECT  {_Primary},{_Field} FROM {_Table} WHERE {_Primary}=@{_Primary} LIMIT 1";
            }
            var par = new DynamicParameters();
            par.Add("@" + _Primary, id);
            return await dapperHelper.FindAsync<T>(sql, par);
        }
        public async Task<T> FindAsync(string where, object par, string field = "*")
        {
            var sql = new StringBuilder();
            sql.Append("SELECT ");
            if (dapperHelper.ConnectionConfig.Type == DbType.sqlserver)
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
            return await dapperHelper.FindAsync<T>(sql.ToString(), par);
        }
        public Task<IEnumerable<T>> FilterAsync()
        {
            var sql = $"SELECT {_Primary},{_Field} FROM {_Table}";
            return dapperHelper.FilterAsync<T>(sql, null);
        }
        public async Task<IEnumerable<T>> FilterAsync(int top, string where, object par, string order = "", string field = "*")
        {
            var sql = new StringBuilder();
            sql.Append("SELECT ");
            if (dapperHelper.ConnectionConfig.Type == DbType.sqlserver)
            {
                sql.Append($" TOP {top}");
            }
            if (field.IsStringEmpty() || field == "*")
            {
                sql.AppendFormat(" {0},{1} ", _Primary, _Field);
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

            if (dapperHelper.ConnectionConfig.Type == DbType.mysql)
            {
                sql.Append($" Limit {top}");
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
            var sql = $"DELETE FROM {_Table} WHERE {_Primary}=@{_Primary}";
            var par = new DynamicParameters();
            par.Add("@" + _Primary, id);
            return await dapperHelper.ExecuteAsync(sql, par);
        }
        public async Task<int> DelAsync(string where, object par)
        {
            if (where.Trim().IsStringEmpty())
            {
                return 0;
            }
            var sql=$"DELETE FROM {_Table}  WHERE {where}";
            return await dapperHelper.ExecuteAsync(sql, par);
        }
        public async Task<M> InsertAsync<M>(T info)
        {
            var regex = new Regex(@"(?<=\])\.\w+\(\)\w+(?=\,)");
            var strSql = new StringBuilder();
            if (_IsIdentity)
            {
              
                strSql.Append($"INSERT INTO {_Table}({ regex.Replace(_Field, "")}) VALUES ({_FieldAt}) ;");
                if (dapperHelper.ConnectionConfig.Type==DbType.sqlserver)
                {
                    strSql.Append("SELECT  @@IDENTITY");
                }
                if (dapperHelper.ConnectionConfig.Type == DbType.mysql)
                {
                    strSql.Append("SELECT LAST_INSERT_ID();");
                }
            }
            else
            {
                strSql.AppendFormat("INSERT INTO {0}({1}) VALUES ({2}) ;", _Table, _Primary + "," + regex.Replace(_Field, ""), "@" + _Primary + "," + _FieldAt);
            }
            return await dapperHelper.SingleAsync<M>(strSql.ToString(), info);
        }
        public async Task<int> EditAsync(T info)
        {
            var strSql = new StringBuilder();
            strSql.AppendFormat("UPDATE {0} SET {1}", _Table, _FieldUp);
            strSql.AppendFormat(" WHERE {0}=@{0}", _Primary);
            return await dapperHelper.ExecuteAsync(strSql.ToString(), info);
        }
        public async Task<int> EditAsync(string val, object par)
        {
            var sql = new StringBuilder();
            sql.AppendFormat("UPDATE  {0} SET {1}", _Table, val);
            return await dapperHelper.ExecuteAsync(sql.ToString(), par);
        }
        public async Task<int> EditAsync(string where, string val, object par)
        {
            var sql = new StringBuilder();
            sql.AppendFormat("UPDATE  {0} SET {1}", _Table, val);
            if (!where.IsStringEmpty())
            {
                sql.AppendFormat(" WHERE {0} ", where);
            }
            return await dapperHelper.ExecuteAsync(sql.ToString(), par);
        }
        #endregion
    }
}
