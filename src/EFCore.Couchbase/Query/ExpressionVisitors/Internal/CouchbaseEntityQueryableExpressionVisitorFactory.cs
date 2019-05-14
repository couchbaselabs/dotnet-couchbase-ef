// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Couchbase.Query.Internal;
using Microsoft.EntityFrameworkCore.Couchbase.Query.Sql;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors;
using Remotion.Linq.Clauses;

namespace Microsoft.EntityFrameworkCore.Couchbase.Query.ExpressionVisitors.Internal
{
    public class CouchbaseEntityQueryableExpressionVisitorFactory : IEntityQueryableExpressionVisitorFactory
    {
        private readonly IModel _model;
        private readonly IEntityMaterializerSource _entityMaterializerSource;
        private readonly ISqlGeneratorFactory _sqlGeneratorFactory;

        public CouchbaseEntityQueryableExpressionVisitorFactory(
            IModel model,
            IEntityMaterializerSource entityMaterializerSource,
            ISqlGeneratorFactory sqlGeneratorFactory)
        {
            _model = model;
            _entityMaterializerSource = entityMaterializerSource;
            _sqlGeneratorFactory = sqlGeneratorFactory;
        }

        public ExpressionVisitor Create(EntityQueryModelVisitor entityQueryModelVisitor, IQuerySource querySource)
            => new CouchbaseEntityQueryableExpressionVisitor(
                _model,
                _entityMaterializerSource,
                (CouchbaseQueryModelVisitor)entityQueryModelVisitor,
                querySource,
                _sqlGeneratorFactory);
    }
}
