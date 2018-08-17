using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Dapper;
using XDF.DapperLambda.Helper;
using XDF.DapperLambda.Model;

namespace XDF.DapperLambda
{
    /// <summary>
    /// 指令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class  CommandSet<T> 
    {
        protected readonly SqlProvider<T> SqlProvider;
        protected readonly IDbConnection DbCon;
        private readonly IDbTransaction _dbTransaction;
        protected DataBaseContext<T> SetContext { get; set; }


        internal Type TableType { get; set; }

        internal LambdaExpression WhereExpression { get; set; }

        internal LambdaExpression IfNotExistsExpression { get; set; }

        public CommandSet(IDbConnection conn, SqlProvider<T> sqlProvider)
        {
            SqlProvider = sqlProvider;
            DbCon = conn;
            TableType = typeof(T);
            SetContext = new DataBaseContext<T>
            {
                CommandSet = this,
                OperateType = EOperateType.Command
            };
            sqlProvider.Context = SetContext;

        }

        public CommandSet(IDbConnection conn, SqlProvider<T> sqlProvider, IDbTransaction dbTransaction)
        {
            SqlProvider = sqlProvider;
            DbCon = conn;
            _dbTransaction = dbTransaction;
            TableType = typeof(T);
            SetContext = new DataBaseContext<T>
            {
                CommandSet = this,
                OperateType = EOperateType.Command
            };

            sqlProvider.Context = SetContext;
        }

        public int Update(T entity)
        {
            SqlProvider.FormatUpdate(a => entity);

            return DbCon.Execute(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }

        public int Update(Expression<Func<T, T>> updateExpression)
        {
            SqlProvider.FormatUpdate(updateExpression);

            return DbCon.Execute(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }

        public int Delete()
        {
            SqlProvider.FormatDelete();

            return DbCon.Execute(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }

        public int Insert(T entity)
        {
            SqlProvider.FormatInsert(entity);

            return DbCon.Execute(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }
        public CommandSet<T> Where(Expression<Func<T, bool>> predicate)
        {
            WhereExpression = WhereExpression == null ? predicate : ((Expression<Func<T, bool>>)WhereExpression).And(predicate);

            return this;
        }

        public CommandSet<T> IfNotExists(Expression<Func<T, bool>> predicate)
        {
            IfNotExistsExpression = IfNotExistsExpression == null ? predicate : ((Expression<Func<T, bool>>)IfNotExistsExpression).And(predicate);

            return this;
        }
        
    }
}