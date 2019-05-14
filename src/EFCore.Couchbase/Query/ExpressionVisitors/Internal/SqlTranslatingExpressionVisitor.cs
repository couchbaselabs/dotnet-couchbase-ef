﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Couchbase.Query.Expressions.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Expressions.Internal;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Remotion.Linq.Clauses.Expressions;

namespace Microsoft.EntityFrameworkCore.Couchbase.Query.ExpressionVisitors.Internal
{
    public class SqlTranslatingExpressionVisitor : ExpressionVisitorBase
    {
        private readonly SelectExpression _selectExpression;
        private readonly QueryCompilationContext _queryCompilationContext;

        public SqlTranslatingExpressionVisitor(
            SelectExpression selectExpression,
            QueryCompilationContext queryCompilationContext)
        {
            _selectExpression = selectExpression;
            _queryCompilationContext = queryCompilationContext;
        }

        public virtual bool Translated { get; private set; } = true;

        protected override Expression VisitMember(MemberExpression memberExpression)
        {
            var properties = MemberAccessBindingExpressionVisitor.GetPropertyPath(
                memberExpression,
                _queryCompilationContext, out var qsre);

            var newExpression = _selectExpression.BindPropertyPath(qsre, properties);
            if (newExpression == null)
            {
                Translated = false;
                return memberExpression;
            }

            return newExpression;
        }

        protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
            var properties = MemberAccessBindingExpressionVisitor.GetPropertyPath(
                methodCallExpression,
                _queryCompilationContext, out var qsre);

            var newExpression = _selectExpression.BindPropertyPath(qsre, properties);
            if (newExpression == null)
            {
                Translated = false;
                return methodCallExpression;
            }

            return newExpression;
        }

        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            var newExpression = Visit(node.Expression);
            if (!Translated)
            {
                return node;
            }

            var castEntityType = _queryCompilationContext.Model.FindEntityType(node.TypeOperand);
            if (castEntityType == null)
            {
                Translated = false;
                return node;
            }

            var discriminatorPredicate = _selectExpression.GetDiscriminatorPredicate(castEntityType);
            if (discriminatorPredicate == null)
            {
                Translated = false;
                return node;
            }

            return discriminatorPredicate;
        }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        protected override Expression VisitExtension(Expression node)
        {
            if (node is NullConditionalExpression nullConditionalExpression)
            {
                var newExpression = Visit(nullConditionalExpression.AccessOperation);
                if (newExpression.Type != node.Type)
                {
                    newExpression = Expression.Convert(newExpression, node.Type);
                }

                return newExpression;
            }

            if (node is NullSafeEqualExpression nullConditionalEqualExpression)
            {
                return Visit(nullConditionalEqualExpression.EqualExpression);
            }

            return base.VisitExtension(node);
        }

        protected override Expression VisitNew(NewExpression expression)
        {
            if (expression.Type == typeof(AnonymousObject)
                || expression.Type == typeof(MaterializedAnonymousObject)
                || expression.Type.IsAnonymousType()
                || expression.Type.IsTupleType())
            {
                Translated = false;
                return expression;
            }

            return base.VisitNew(expression);
        }

        protected override Expression VisitConstant(ConstantExpression expression)
        {
            if (expression.Type == typeof(AnonymousObject)
                || expression.Type == typeof(MaterializedAnonymousObject)
                || expression.Type.IsAnonymousType()
                || expression.Type.IsTupleType())
            {
                Translated = false;
                return expression;
            }

            return base.VisitConstant(expression);
        }

        protected override Expression VisitSubQuery(SubQueryExpression subQueryExpression)
        {
            Translated = false;
            return subQueryExpression;
        }

        protected override Expression VisitQuerySourceReference(QuerySourceReferenceExpression expression)
        {
            var newExpression = _selectExpression.BindPropertyPath(expression, new List<IPropertyBase>());
            if (newExpression == null)
            {
                Translated = false;
                return expression;
            }

            return newExpression;
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            var newOperand = Visit(node.Operand);
            return (node.NodeType == ExpressionType.Convert
                    || node.NodeType == ExpressionType.ConvertChecked)
                   && newOperand?.Type == node.Type
                ? newOperand
                : node.Update(newOperand);
        }
    }
}
