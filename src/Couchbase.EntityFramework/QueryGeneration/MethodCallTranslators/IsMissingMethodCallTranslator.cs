﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Couchbase.EntityFramework.QueryGeneration.MethodCallTranslators
{
    internal class IsMissingMethodCallTranslator : IMethodCallTranslator
    {
        private static readonly MethodInfo[] SupportedMethodsStatic =
            typeof (N1QlFunctions).GetMethods().Where(p => (p.Name == "IsMissing") || (p.Name == "IsNotMissing")).ToArray();

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

            expressionTreeVisitor.Visit(methodCallExpression.Arguments[0]);

            if (methodCallExpression.Arguments.Count > 1)
            {
                var constantExpression = methodCallExpression.Arguments[1] as ConstantExpression;
                if (constantExpression == null)
                {
                    throw new NotSupportedException("IsMissing and IsNotMissing propertyName parameter must be a constant");
                }

                expression.AppendFormat(".{0}",
                    N1QlHelpers.EscapeIdentifier(constantExpression.Value.ToString()));
            }

            expression.Append(methodCallExpression.Method.Name == "IsMissing" ? " IS MISSING" : " IS NOT MISSING");

            return methodCallExpression;
        }
    }
}
