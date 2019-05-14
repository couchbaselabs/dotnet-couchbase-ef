// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Couchbase.Storage.Internal;
using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.Couchbase.Query.Internal
{
    public class CouchbaseQueryContextFactory : QueryContextFactory
    {
        private readonly CouchbaseClientWrapper _CouchbaseClient;

        public CouchbaseQueryContextFactory(
            [NotNull] QueryContextDependencies dependencies,
            [NotNull] CouchbaseClientWrapper CouchbaseClient)
            : base(dependencies)
        {
            _CouchbaseClient = CouchbaseClient;
        }

        public override QueryContext Create()
            => new CouchbaseQueryContext(Dependencies, CreateQueryBuffer, _CouchbaseClient);
    }
}
