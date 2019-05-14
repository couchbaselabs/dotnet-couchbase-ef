// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Couchbase.TestUtilities
{
    public class CouchbaseTestStoreFactory : ITestStoreFactory
    {
        public static CouchbaseTestStoreFactory Instance { get; } = new CouchbaseTestStoreFactory();

        protected CouchbaseTestStoreFactory()
        {
        }

        public IServiceCollection AddProviderServices(IServiceCollection serviceCollection)
            => serviceCollection
                .AddEntityFrameworkCouchbase()
                .AddSingleton<ILoggerFactory>(new TestSqlLoggerFactory())
                .AddSingleton<TestStoreIndex>();

        public TestStore Create(string storeName) => CouchbaseTestStore.Create(storeName);

        public virtual TestStore GetOrCreate(string storeName) => CouchbaseTestStore.GetOrCreate(storeName);

        public virtual ListLoggerFactory CreateListLoggerFactory(Func<string, bool> shouldLogCategory)
            => new TestSqlLoggerFactory(shouldLogCategory);
    }
}
