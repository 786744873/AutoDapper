using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using XDF.Core.Helper.JsonConfig;
using XDF.Core.Helper.Log;
using XDF.Core.Model;

namespace XDF.Core.Helper.Db
{
    public class DapperHelper : IDisposable
    {
        public DbConnectionConfig ConnectionConfig => JsonConfigHelper.GetDbConnectionStr(_DbName);

        private readonly string _DbName;
        public IDbConnection Db
        {

            get
            {
                if (ConnectionConfig.Type == JsonConfig.DbType.sqlserver)
                {
                    return new SqlConnection(ConnectionConfig.ConnectionString);
                }
                return new MySqlConnection(ConnectionConfig.ConnectionString);
            }
        }
        public DapperHelper(string dbName)
        {
            _DbName = dbName;

        }
        public void Dispose()
        {
            if (Db.State == ConnectionState.Open)
            {
                Db.Close();
            }
        }
        public T Find<T>(string sql, object par, CommandType commandType = CommandType.Text)
        {
            using (Db)
            {
                LogHelper.Sql(sql);
                return Db.QueryFirstOrDefault<T>(sql, par, commandType: commandType);
            }
        }
        public async Task<T> FindAsync<T>(string sql, object par, CommandType commandType = CommandType.Text)
        {
            using (Db)
            {
                return await Db.QueryFirstOrDefaultAsync<T>(sql, par, commandType: commandType);
            }
        }
        /// <summary>
        ///     查找集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="par"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public List<T> Filter<T>(string sql, object par, CommandType commandType = CommandType.Text)
        {
            using (Db)
            {
                LogHelper.Sql(sql);
                return Db.Query<T>(sql, par, commandType: commandType).ToList();
            }
        }
        /// <summary>
        ///     查找集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="par"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> FilterAsync<T>(string sql, object par, CommandType commandType = CommandType.Text)
        {
            using (Db)
            {
                LogHelper.Sql(sql);
                return await Db.QueryAsync<T>(sql, par, commandType: commandType);
            }
        }
        /// <summary>
        ///     分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public PageListModel<T> Filter<T>(PageListModel<T> info)
        {
            using (Db)
            {
                var sqlCount = new StringBuilder();
                sqlCount.Append("SELECT count(1) FROM " + info.Table);
                var sql = new StringBuilder();
                sql.Append("SELECT " + info.Field + " FROM " + info.Table);
                var where = new StringBuilder();
                if (!info.WhereCol.IsStringEmpty() && !info.WhereVal.IsStringEmpty())
                {
                    var fildn = info.WhereCol.Trim().ToLower();
                    if (fildn.Contains("."))
                    {
                        fildn = fildn.Split('.')[1];
                    }
                    info.WhereVal = info.WhereVal.Trim();
                    switch (info.WhereCon)
                    {
                        //等于
                        case "0":
                            where.Append(" AND " + info.WhereCol.EvalSqlString() + "='" + info.WhereVal.EvalSqlString() + "'");
                            break;
                        //包含
                        case "1":
                            where.Append(" AND " + info.WhereCol.EvalSqlString() + " LIKE '%" + info.WhereVal.EvalSqlString() + "%' ");
                            break;
                        //开始于
                        case "2":
                            where.Append(" AND " + info.WhereCol.EvalSqlString() + " LIKE '" + info.WhereVal + "%'");
                            break;
                        //结束于
                        case "3":
                            where.Append(" AND " + info.WhereCol.EvalSqlString() + " LIKE '%" + info.WhereVal + "'");
                            break;
                        //不包含
                        case "4":
                            where.Append(" AND " + info.WhereCol.EvalSqlString() + " NOT LIKE '%" + info.WhereVal.EvalSqlString() + "%' ");
                            break;
                        //大于
                        case "5":
                            where.Append(" AND " + info.WhereCol.EvalSqlString() + ">'" + info.WhereVal.EvalSqlString() + "'");
                            break;
                        //大于等于
                        case "6":
                            where.Append(" AND " + info.WhereCol.EvalSqlString() + ">='" + info.WhereVal.EvalSqlString() + "'");
                            break;
                        //小于
                        case "7":
                            where.Append(" AND " + info.WhereCol.EvalSqlString() + "<'" + info.WhereVal.EvalSqlString() + "'");
                            break;
                        //小于等于
                        case "8":
                            where.Append(" AND " + info.WhereCol.EvalSqlString() + "<='" + info.WhereVal.EvalSqlString() + "'");
                            break;
                        //介于
                        case "9":
                            where.Append(" AND " + info.WhereCol.EvalSqlString() + " BETWEEN '" + info.WhereVal.EvalSqlString() + "' AND '" + info.WhereVal1.EvalSqlString() + "'");
                            break;
                        default:
                            break;
                    }
                }
                if (info.PageIndex == 0)
                {
                    info.PageIndex = 1;
                }
                if (!info.WhereStr.IsStringEmpty())
                {
                    where.Append(" AND " + info.WhereStr);
                }
                var tempWhere = where.ToString().Trim().TrimStart("AND".ToCharArray());
                if (!string.IsNullOrWhiteSpace(tempWhere))
                {
                    sqlCount.Append(" WHERE " + tempWhere);
                    sql.Append(" WHERE " + tempWhere);
                }
                if (!info.GroupBy.IsStringEmpty())
                {
                    sql.Append(" GROUP BY " + info.GroupBy);
                }
                if (info.IsGetCount)
                {
                    info.Count = !info.GroupBy.IsStringEmpty() ? Db.ExecuteScalar<int>(string.Format("SELECT COUNT(1) FROM({0})as tmp", sql.ToString())) : Db.ExecuteScalar<int>(sqlCount.ToString(), info);
                }
                if (info.OrderBy.IsStringEmpty())
                {
                    if (!info.Order.IsStringEmpty() && !info.Sort.IsStringEmpty())
                    {
                        info.OrderBy = info.Sort + " " + info.Order;
                    }
                    else
                    {
                        info.OrderBy = info.Primary + " DESC";
                    }
                }
                sql.Append(" ORDER BY  " + info.OrderBy);
                if (info.IsPage)
                {
                    info.PageCount = info.PageSize == 0 ? 0 : Math.Ceiling(info.Count.ToDecimal() / info.PageSize.ToDecimal()).ToInt();

                    sql.Append(ConnectionConfig.Type == JsonConfig.DbType.sqlserver ? " OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY " : " limit @Offset,@Limit");
                    if (info.Offset.ToInt() <= 0)
                    {
                        info.Offset = (info.PageIndex - 1) * info.PageSize;
                    }
                    if (info.Limit.ToInt() <= 0)
                    {
                        info.Limit = info.PageSize;
                    }
                }
                var s = sql.ToString();
                info.List = Db.Query<T>(sql.ToString(), info).ToList();
                return info;
            }
        }
        /// <summary>
        ///     分页。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlStr"></param>
        /// <param name="par"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<T> Filter<T>(string sqlStr, object par, string orderBy, int pageIndex, int pageSize)
        {
            using (Db)
            {
                var sql = new StringBuilder();
                sql.Append(sqlStr);
                var index = (pageIndex - 1) * pageSize;
                if (!orderBy.IsStringEmpty())
                {
                    sql.Append(" ORDER BY " + orderBy);
                }
                sql.AppendFormat(" LIMIT {0},{1}", index, pageSize);
                return Db.Query<T>(sql.ToString(), par).ToList();
            }
        }
        public async Task<IEnumerable<T>> FilterAsync<T>(string sqlStr, object par, string orderBy, int pageIndex, int pageSize)
        {
            using (Db)
            {
                var sql = new StringBuilder();
                sql.Append(sqlStr);
                var index = (pageIndex - 1) * pageSize;
                if (!orderBy.IsStringEmpty())
                {
                    sql.Append(" ORDER BY " + orderBy);
                }
                sql.AppendFormat(" LIMIT {0},{1}", index, pageSize);
                return await Db.QueryAsync<T>(sql.ToString(), par);
            }
        }
        /// <summary>
        ///     SQL事务操作Dictionary<string, object>  string sql object 参数
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        public bool SqlTran(Dictionary<string, object> par)
        {
            using (Db)
            {
                Db.Open();
                using (var transaction = Db.BeginTransaction())
                {
                    try
                    {
                        foreach (var m in par)
                        {
                            Db.Execute(m.Key, m.Value, transaction, null, null);
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                    finally
                    {
                        Db.Close();
                    }
                }
            }
        }
        /// <summary>
        /// 批量执行sql语句 并返回List结果
        /// </summary>
        /// <param name="par">sql语句，参数</param>
        /// <returns></returns>
        public List<dynamic> SqlTranList(Dictionary<string, object> par)
        {
            var result = new List<dynamic>();
            using (Db)
            {
                Db.Open();
                using (var transaction = Db.BeginTransaction())
                {
                    try
                    {
                        foreach (var m in par)
                        {
                            result.Add(Db.Query<dynamic>(m.Key, m.Value, transaction));
                        }
                        transaction.Commit();
                        return result;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return null;
                    }
                    finally
                    {
                        Db.Close();
                    }
                }
            }
        }
        /// <summary>
        ///     执行SQL语句或存储过程
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="par"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int Execute(string sql, object par, CommandType type = CommandType.Text)
        {
            using (Db)
            {
                return Db.Execute(sql, par, commandType: type);
            }
        }
        public async Task<int> ExecuteAsync(string sql, object par, CommandType type = CommandType.Text)
        {
            using (Db)
            {
                return await Db.ExecuteAsync(sql, par, commandType: type);
            }
        }
        public T ExecuteScalar<T>(string sql, object par, CommandType type = CommandType.Text)
        {
            using (Db)
            {
                var result = Db.ExecuteScalar<T>(sql, par, commandType: type);
                return result;
            }
        }
        public List<T> ExecuteScalarList<T>(string sql, object par, CommandType type = CommandType.Text)
        {
            using (Db)
            {
                var result = Db.ExecuteScalar<List<T>>(sql, par, commandType: type);
                return result;
            }
        }
        public async Task<T> ExecuteScalarAsync<T>(string sql, object par, CommandType type = CommandType.Text)
        {
            using (Db)
            {
                return await Db.ExecuteScalarAsync<T>(sql, par, commandType: type);
            }
        }
        /// <summary>
        ///     返回一个Scalar查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="par"></param>
        /// <returns></returns>
        public T Single<T>(string sql, object par)
        {
            using (Db)
            {
                return Db.ExecuteScalar<T>(sql, par);
            }
        }
        public async Task<T> SingleAsync<T>(string sql, object par)
        {
            using (Db)
            {
                return await Db.ExecuteScalarAsync<T>(sql, par);
            }
        }
    }
}
