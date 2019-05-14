// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.Couchbase.Query.Internal
{
    public class CouchbaseEntityQueryModelVisitorFactory : EntityQueryModelVisitorFactory
    {
        public CouchbaseEntityQueryModelVisitorFactory([NotNull] EntityQueryModelVisitorDependencies dependencies)
            : base(dependencies)
        {
        }

        public override EntityQueryModelVisitor Create(
            QueryCompilationContext queryCompilationContext, EntityQueryModelVisitor parentEntityQueryModelVisitor)
        {
            return new CouchbaseQueryModelVisitor(Dependencies, queryCompilationContext);
        }
    }
}
