﻿using System.Linq.Expressions;
using System.Reflection;
using Couchbase.EntityFramework.Extensions;
using Remotion.Linq.Clauses;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace Couchbase.EntityFramework.Operators
{
    /// <summary>
    /// Expression parser for N1QL "Explain" method.
    /// </summary>
    internal class ExplainExpressionNode : ResultOperatorExpressionNodeBase
    {
        public static MethodInfo[] SupportedMethods =
        {
            typeof (QueryExtensions).GetMethod("Explain")
        };

        public ExplainExpressionNode(MethodCallExpressionParseInfo parseInfo,
            LambdaExpression optionalPredicate,
            LambdaExpression optionalSelector)
            : base(parseInfo, optionalPredicate, optionalSelector)
        {
        }

        protected override ResultOperatorBase CreateResultOperator(
            ClauseGenerationContext clauseGenerationContext)
        {
            return new ExplainResultOperator();
        }

        public override Expression Resolve(ParameterExpression inputParameter,
            Expression expressionToBeResolved,
            ClauseGenerationContext clauseGenerationContext)
        {
            return Source.Resolve(
                inputParameter,
                expressionToBeResolved,
                clauseGenerationContext);
        }
    }
}
