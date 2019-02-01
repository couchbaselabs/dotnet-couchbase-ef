using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Couchbase.EntityFramework.QueryGeneration.MethodCallTranslators
{
    internal class ListIndexMethodCallTranslator : IMethodCallTranslator
    {
        private static readonly MethodInfo[] SupportedMethodsStatic = {
            typeof (IList).GetMethod("get_Item"),
            typeof (IList<>).GetMethod("get_Item")
        };

        public IEnumerable<MethodInfo> SupportMethods
        {
            get
            {
                return SupportedMethodsStatic;
            }
        }

        public Expression Translate(MethodCallExpression methodCallExpression, N1QlExpressionTreeVisitor expressionTreeVisitor)
        {
            if (methodCallExpression == null)
            {
                throw new ArgumentNullException("methodCallExpression");
            }

            var expression = expressionTreeVisitor.Expression;

            expressionTreeVisitor.Visit(methodCallExpression.Object);
            expression.Append('[');
            expressionTreeVisitor.Visit(methodCallExpression.Arguments[0]);
            expression.Append(']');

            return methodCallExpression;
        }
    }
}
