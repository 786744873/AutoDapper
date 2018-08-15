using System;
using System.Linq.Expressions;

namespace XDF.DapperLambda.Interface
{
    public interface ICommand<T>
    {
        int Insert(T entity);
        int Update(T entity);
        int Update(Expression<Func<T, T>> updateExpression);
        int Delete();
    }
}