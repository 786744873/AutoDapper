﻿using System;
using System.Linq.Expressions;
using XDF.DapperLambda.Helper;

namespace XDF.DapperLambda.Expressions
{
    internal class TrimExpression : ExpressionVisitor
    {
        private bool IsDeep = false;
        internal static Expression Trim(Expression expression)
        {
            return new TrimExpression().Visit(expression);
        }

        private Expression Sub(Expression expression)
        {
            var type = expression.Type;
            switch (expression.NodeType)
            {
                case ExpressionType.Equal:
                case ExpressionType.Call:
                    IsDeep = true;
                    return expression;

                case ExpressionType.Constant:
                    if (TypeHelper.GetNonNullableType(expression.Type) == TypeHelper.GetNonNullableType(type))
                        return Expression.Constant(((ConstantExpression)expression).Value, type);
                    break;

                case ExpressionType.MemberAccess:
                    return expression;
                    var mExpression = expression as MemberExpression;
                    var root = mExpression.GetRootMember();
                    if (root != null)
                    {
                        var value = mExpression.MemberToValue(root);
                        return Expression.Constant(value, type);
                    }
                    else
                    {
                        if (IsDeep)
                            return expression;

                        IsDeep = true;
                        return Expression.Equal(expression, Expression.Constant(true));
                    }

                case ExpressionType.Convert:
                    var u = (UnaryExpression)expression;
                    if (TypeHelper.GetNonNullableType(u.Operand.Type) == TypeHelper.GetNonNullableType(type))
                    {
                        expression = u.Operand;
                        return expression;
                    }

                    if (u.Operand.Type.IsEnum && u.Operand.NodeType == ExpressionType.MemberAccess)
                    {
                        var value = Convert.ChangeType((u.Operand as MemberExpression).MemberToValue(), type);
                        return Expression.Constant(value, type);
                    }
                    break;

                case ExpressionType.Not:
                    var n = (UnaryExpression)expression;
                    return Expression.Equal(n.Operand, Expression.Constant(false));
                case ExpressionType.AndAlso:
                    var b = (BinaryExpression)expression;
                    IsDeep = true;
                    if (b.Left.NodeType != b.Right.NodeType)
                    {
                        if (b.Left.NodeType == ExpressionType.MemberAccess && b.Left.Type.Name == "Boolean")
                        {
                            return Expression.AndAlso(Expression.Equal(b.Left, Expression.Constant(true)), b.Right);
                        }
                        if (b.Right.NodeType == ExpressionType.MemberAccess && b.Right.Type.Name == "Boolean")
                        {
                            return Expression.AndAlso(b.Left, Expression.Equal(b.Right, Expression.Constant(true)));
                        }
                        if (b.Left.NodeType == ExpressionType.Constant)
                            return b.Right;
                        if (b.Right.NodeType == ExpressionType.Constant)
                            return b.Left;
                    }

                    break;
            }

            return expression;
        }

        public override Expression Visit(Expression exp)
        {
            if (exp == null)
                return null;

            exp = Sub(exp);
            return base.Visit(exp);
        }
    }
}
