using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Couchbase.EntityFramework.QueryGeneration.MethodCallTranslators
{
    internal class StringIndexOfMethodCallTranslator : IMethodCallTranslator
    {
        private static readonly MethodInfo[] SupportedMethodsStatic =
        {
            typeof (string).GetMethod("IndexOf", new[] { typeof (char) }),
            typeof (string).GetMethod("IndexOf", new[] { typeof (string) })
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

            expression.Append("POSITION(");
            expressionTreeVisitor.Visit(methodCallExpression.Object);
            expression.Append(", ");
            expressionTreeVisitor.Visit(methodCallExpression.Arguments[0]);
            expression.Append(")");

            return methodCallExpression;
        }
    }
}
