﻿using System.Linq.Expressions;
using System.Reflection;
using Couchbase.EntityFramework.QueryGeneration;
using Remotion.Linq.Clauses;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace Couchbase.EntityFramework.Operators
{
    /// <summary>
    /// Expression parser for LinqQueryRequest.ToQueryRequest method.
    /// </summary>
    internal class ToQueryRequestExpressionNode : ResultOperatorExpressionNodeBase
    {
        public static readonly MethodInfo[] SupportedMethods =
        {
            typeof (LinqQueryRequest).GetMethod("ToQueryRequest", BindingFlags.NonPublic | BindingFlags.Static)
        };

        public ToQueryRequestExpressionNode(MethodCallExpressionParseInfo parseInfo)
            : base(parseInfo, null, null)
        {
        }

        protected override ResultOperatorBase CreateResultOperator(
            ClauseGenerationContext clauseGenerationContext)
        {
            return new ToQueryRequestResultOperator();
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
