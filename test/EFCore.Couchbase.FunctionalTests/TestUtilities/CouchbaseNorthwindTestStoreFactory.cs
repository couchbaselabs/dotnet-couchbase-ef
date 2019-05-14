// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.TestUtilities;

namespace Microsoft.EntityFrameworkCore.Couchbase.TestUtilities
{
    public class CouchbaseNorthwindTestStoreFactory : CouchbaseTestStoreFactory
    {
        private const string Name = "Northwind";

        public static new CouchbaseNorthwindTestStoreFactory Instance { get; }
            = new CouchbaseNorthwindTestStoreFactory();

        protected CouchbaseNorthwindTestStoreFactory()
        {
        }

        public override TestStore GetOrCreate(string storeName)
            => CouchbaseTestStore.GetOrCreate(Name, "Northwind.json");
    }
}
