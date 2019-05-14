// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Couchbase.Storage.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Microsoft.EntityFrameworkCore.Couchbase.Query
{
    public class CouchbaseQueryContext : QueryContext
    {
        public CouchbaseQueryContext(
            [NotNull] QueryContextDependencies dependencies,
            [NotNull] Func<IQueryBuffer> queryBufferFactory,
            [NotNull] CouchbaseClientWrapper couchbaseClient)
            : base(dependencies, queryBufferFactory)
        {
            this.CouchbaseClient = couchbaseClient;
        }

        public CouchbaseClientWrapper CouchbaseClient { get; }
    }
}
