// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Couchbase.Internal;
using Microsoft.EntityFrameworkCore.Couchbase.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Couchbase.Query.Expressions.Internal;
using Microsoft.EntityFrameworkCore.Couchbase.Query.Internal;
using Microsoft.EntityFrameworkCore.Couchbase.Query.Sql;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors;
using Remotion.Linq.Clauses;

namespace Microsoft.EntityFrameworkCore.Couchbase.Query.ExpressionVisitors.Internal
{
    public class CouchbaseEntityQueryableExpressionVisitor : EntityQueryableExpressionVisitor
    {
        private readonly IModel _model;
        private readonly IQuerySource _querySource;
        private readonly IEntityMaterializerSource _entityMaterializerSource;
        private readonly ISqlGeneratorFactory _sqlGeneratorFactory;

        public CouchbaseEntityQueryableExpressionVisitor(
            IModel model,
            IEntityMaterializerSource entityMaterializerSource,
            CouchbaseQueryModelVisitor CouchbaseQueryModelVisitor,
            IQuerySource querySource,
            ISqlGeneratorFactory sqlGeneratorFactory)
            : base(CouchbaseQueryModelVisitor)
        {
            _model = model;
            _querySource = querySource;
            _entityMaterializerSource = entityMaterializerSource;
            _sqlGeneratorFactory = sqlGeneratorFactory;
        }

        public new CouchbaseQueryModelVisitor QueryModelVisitor => (CouchbaseQueryModelVisitor)base.QueryModelVisitor;

        protected override Expression VisitEntityQueryable([NotNull] Type elementType)
        {
            var entityType = _model.FindEntityType(elementType);
            Debug.Assert(entityType.IsDocumentRoot());

            return new QueryShaperExpression(
                QueryModelVisitor.QueryCompilationContext.IsAsyncQuery,
                new DocumentQueryExpression(
                    QueryModelVisitor.QueryCompilationContext.IsAsyncQuery,
                    entityType.Couchbase().ContainerName,
                    new SelectExpression(entityType, _querySource, _sqlGeneratorFactory)),
                new EntityShaper(
                    entityType,
                    trackingQuery: QueryModelVisitor.QueryCompilationContext.IsTrackingQuery
                                   && entityType.FindPrimaryKey() != null,
                    useQueryBuffer: QueryModelVisitor.QueryCompilationContext.IsQueryBufferRequired
                                    && entityType.FindPrimaryKey() != null,
                    _entityMaterializerSource));
        }
    }
}
