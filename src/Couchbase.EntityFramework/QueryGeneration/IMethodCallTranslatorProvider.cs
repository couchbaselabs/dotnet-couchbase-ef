using System.Linq.Expressions;

namespace Couchbase.EntityFramework.QueryGeneration
{
    internal interface IMethodCallTranslatorProvider
    {
        IMethodCallTranslator GetTranslator(MethodCallExpression methodCallExpression);
    }
}
