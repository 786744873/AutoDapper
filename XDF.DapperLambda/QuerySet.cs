using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using XDF.DapperLambda.Helper;
using XDF.DapperLambda.Model;

namespace XDF.DapperLambda
{
    public class QuerySet<T>
    {
        protected readonly SqlProvider<T> SqlProvider;
        protected readonly IDbConnection DbCon;
        protected readonly IDbTransaction DbTransaction;
        internal LambdaExpression SelectExpression { get; set; }
        internal Dictionary<EOrderBy, LambdaExpression> OrderbyExpressionList { get; set; }
        internal int? TopNum { get; set; }
        protected DataBaseContext<T> SetContext { get; set; }
        internal Type TableType { get; set; }

        internal LambdaExpression WhereExpression { get; set; }

        public QuerySet(IDbConnection conn, SqlProvider<T> sqlProvider) 
        {
            SqlProvider = sqlProvider;
            DbCon = conn;
            OrderbyExpressionList = new Dictionary<EOrderBy, LambdaExpression>();
            TableType = typeof(T);
            SetContext = new DataBaseContext<T>
            {
                QuerySet = this,
                OperateType = EOperateType.Query
            };

            sqlProvider.Context = SetContext;
        }

        public QuerySet(IDbConnection conn, SqlProvider<T> sqlProvider, IDbTransaction dbTransaction) 
        {
            SqlProvider = sqlProvider;
            DbCon = conn;
            DbTransaction = dbTransaction;
            OrderbyExpressionList = new Dictionary<EOrderBy, LambdaExpression>();
            TableType = typeof(T);
            SetContext = new DataBaseContext<T>
            {
                QuerySet = this,
                OperateType = EOperateType.Query
            };

            sqlProvider.Context = SetContext;
        }

        internal QuerySet(IDbConnection conn, SqlProvider<T> sqlProvider, Type tableType, LambdaExpression whereExpression, LambdaExpression selectExpression, int? topNum, Dictionary<EOrderBy, LambdaExpression> orderbyExpressionList, IDbTransaction dbTransaction) 
        {
            TableType = tableType;
            WhereExpression = whereExpression;
            SelectExpression = selectExpression;
            TopNum = topNum;
            OrderbyExpressionList = orderbyExpressionList;

            SetContext = new DataBaseContext<T>
            {
                QuerySet = this,
                OperateType = EOperateType.Query
            };

            sqlProvider.Context = SetContext;
        }
       
        public QuerySet<T> Where(Expression<Func<T, bool>> predicate)
        {
            WhereExpression = WhereExpression == null ? predicate : ((Expression<Func<T, bool>>)WhereExpression).And(predicate);

            return this;
        }
        public int Count()
        {
            SqlProvider.FormatCount();

            return DbCon.QuerySingle<int>(SqlProvider.SqlString, SqlProvider.Params);
        }

        public TResult Sum<TResult>(Expression<Func<T, TResult>> sumExpression)
        {
            SqlProvider.FormatSum(sumExpression);

            return DbCon.QuerySingle<TResult>(SqlProvider.SqlString, SqlProvider.Params);
        }

        public bool Exists()
        {
            SqlProvider.FormatExists();

            return DbCon.QuerySingle<int>(SqlProvider.SqlString, SqlProvider.Params) == 1;
        }
        /// <summary>
        /// 顺序
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public  QuerySet<T> OrderBy<TProperty>(Expression<Func<T, TProperty>> field)
        {
            if (field != null)
                OrderbyExpressionList.Add(EOrderBy.Asc, field);

            return this;
        }

        /// <summary>
        /// 倒叙
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public  QuerySet<T> OrderByDescing<TProperty>(Expression<Func<T, TProperty>> field)
        {
            if (field != null)
                OrderbyExpressionList.Add(EOrderBy.Desc, field);

            return this;
        }
        public  QuerySet<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
        {
            SelectExpression = selector;
            return new QuerySet<TResult>(DbCon, new SqlProvider<TResult>(), typeof(T), WhereExpression,SelectExpression, TopNum, OrderbyExpressionList, DbTransaction);
        }
        public T Get()
        {
            SqlProvider.FormatGet();

            return DbCon.QueryFirstOrDefault<T>(SqlProvider.SqlString, SqlProvider.Params, DbTransaction);
        }

        public List<T> ToList()
        {
            SqlProvider.FormatToList();

            return DbCon.Query<T>(SqlProvider.SqlString, SqlProvider.Params, DbTransaction).ToList();
        }

        public PageList<T> PageList(int pageIndex, int pageSize)
        {
            SqlProvider.FormatToPageList(pageIndex, pageSize);

            using (var queryResult = DbCon.QueryMultiple(SqlProvider.SqlString, SqlProvider.Params, DbTransaction))
            {
                var pageTotal = queryResult.ReadFirst<int>();

                var itemList = queryResult.Read<T>().ToList();

                return new PageList<T>(pageIndex, pageSize, pageTotal, itemList);
            }
        }

        public List<T> UpdateSelect(Expression<Func<T, T>> updator)
        {
            SqlProvider.FormatUpdateSelect(updator);

            return DbCon.Query<T>(SqlProvider.SqlString, SqlProvider.Params, DbTransaction).ToList();
        }
    }
}