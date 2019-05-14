// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using Remotion.Linq.Clauses;

namespace Microsoft.EntityFrameworkCore.Couchbase.Query.ExpressionVisitors.Internal
{
    public class CouchbaseMemberAccessBindingExpressionVisitorFactory : MemberAccessBindingExpressionVisitorFactory
    {
        public override ExpressionVisitor Create(
            QuerySourceMapping querySourceMapping,
            EntityQueryModelVisitor queryModelVisitor,
            bool inProjection)
            => new CouchbaseMemberAccessBindingExpressionVisitor(querySourceMapping, queryModelVisitor, inProjection);
    }
}
