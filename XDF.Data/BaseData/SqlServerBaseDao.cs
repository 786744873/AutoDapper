using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using XDF.Core.Helper.Db;
using XDF.Core.Model;

namespace XDF.Data
{
    public class SqlServerBaseDao<model> : IDisposable where model : class
    {
        public readonly SqlServerDapperHelper dapperHelper;
        ///主建是否自增，如果不自增插入的时候要赋值
        public string _DataBase = "";
        public string _Field = "";
        public string _FieldAt = "";
        public string _FieldUp = "";
        public string _FieldInsert = "";
        public bool _IsIdentity = true;
        public string _Primary = "";
        public string _Table = "";
        public SqlServerBaseDao(string db)
        {
            dapperHelper = new SqlServerDapperHelper(db);
        }
        #region CRUD
        public model Find(object Id)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(SqlServerFormat.Find, _Primary + "," + _Field, _Table, $" WHERE {_Primary}=@{_Primary}");
            var par = new DynamicParameters();
            par.Add("@" + _Primary, Id);
            return dapperHelper.Find<model>(sql.ToString(), par);
        }
        public model Find(string where, object par, string field = "*")
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
            return dapperHelper.Find<model>(sql.ToString(), par);
        }
        public List<model> Filter()
        {
            var sql = new StringBuilder();
            sql.AppendFormat(SqlServerFormat.Filter, _Primary, _Field, _Table);
            return dapperHelper.Filter<model>(sql.ToString(), null);
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
        public List<model> Filter(int top, string where, object par, string order = "", string field = "*")
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
            return dapperHelper.Filter<model>(sql.ToString(), par);
        }
        public List<model> Filter(string where, object par, string order = "", string field = "*")
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
            return dapperHelper.Filter<model>(sql.ToString(), par);
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
        public T Insert<T>(model info)
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
        public int Add(model info)
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
        public int Edit(model info)
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
        public async Task<model> FindAsync(object id)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(SqlServerFormat.Find, _Primary + "," + _Field, _Table, $" WHERE {_Primary}=@{_Primary}");
            var par = new DynamicParameters();
            par.Add("@" + _Primary, id);
            return await dapperHelper.FindAsync<model>(sql.ToString(), par);
        }
        public async Task<model> FindAsync(string where, object par, string field = "*")
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
            return await dapperHelper.FindAsync<model>(sql.ToString(), par);
        }
        public Task<IEnumerable<model>> FilterAsync()
        {
            var sql = new StringBuilder();
            sql.AppendFormat(SqlServerFormat.Filter, _Primary, _Field, _Table);
            return dapperHelper.FilterAsync<model>(sql.ToString(), null);
        }
        public async Task<IEnumerable<model>> FilterAsync(int top, string where, object par, string order = "", string field = "*")
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
            return await dapperHelper.FilterAsync<model>(sql.ToString(), par);
        }
        public async Task<IEnumerable<model>> FilterAsync(string where, object par, string order = "", string field = "*")
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
            return await dapperHelper.FilterAsync<model>(sql.ToString(), par);
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
        public async Task<T> InsertAsync<T>(model info)
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
            return await dapperHelper.SingleAsync<T>(strSql.ToString(), info);
        }
        public async Task<int> EditAsync(model info)
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
