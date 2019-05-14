// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.Pipeline;

namespace Microsoft.EntityFrameworkCore.Couchbase.Query.Pipeline
{
    public class CouchbaseEntityQueryableTranslatorFactory : EntityQueryableTranslatorFactory
    {
        private readonly IModel _model;

        public CouchbaseEntityQueryableTranslatorFactory(IModel model)
        {
            _model = model;
        }

        public override EntityQueryableTranslator Create(QueryCompilationContext2 queryCompilationContext)
        {
            throw new NotImplementedException();
        }
    }

    public class CouchbaseQueryableMethodTranslatingExpressionVisitorFactory : IQueryableMethodTranslatingExpressionVisitorFactory
    {
        public QueryableMethodTranslatingExpressionVisitor Create(QueryCompilationContext2 queryCompilationContext)
        {
            throw new NotImplementedException();
        }
    }

    public class CouchbaseShapedQueryCompilingExpressionVisitorFactory : IShapedQueryCompilingExpressionVisitorFactory
    {
        private readonly IEntityMaterializerSource _entityMaterializerSource;

        public CouchbaseShapedQueryCompilingExpressionVisitorFactory(IEntityMaterializerSource entityMaterializerSource)
        {
            _entityMaterializerSource = entityMaterializerSource;
        }

        public ShapedQueryCompilingExpressionVisitor Create(QueryCompilationContext2 queryCompilationContext)
        {
            throw new NotImplementedException();
            //return new CouchbaseShapedQueryCompilingExpressionVisitor(
            //    _entityMaterializerSource,
            //    queryCompilationContext.TrackQueryResults,
            //    queryCompilationContext.Async);
        }
    }
}
