using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Couchbase.EntityFramework.QueryGeneration.MethodCallTranslators
{
    internal class StringCapitalizationMethodCallTranslator : IMethodCallTranslator
    {
        private static readonly MethodInfo[] SupportedMethodsStatic =
        {
            typeof (string).GetMethod("ToUpper", Type.EmptyTypes),
            typeof (string).GetMethod("ToLower", Type.EmptyTypes)
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

            expression.Append(methodCallExpression.Method.Name == "ToLower" ? "LOWER(" : "UPPER(");
            expressionTreeVisitor.Visit(methodCallExpression.Object);
            expression.Append(")");

            return methodCallExpression;
        }
    }
}
