using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Couchbase.EntityFramework.QueryGeneration
{
    internal interface IMethodCallTranslator
    {
        IEnumerable<MethodInfo> SupportMethods { get; }

        Expression Translate(MethodCallExpression methodCallExpression, N1QlExpressionTreeVisitor expressionTreeVisitor);
    }
}
