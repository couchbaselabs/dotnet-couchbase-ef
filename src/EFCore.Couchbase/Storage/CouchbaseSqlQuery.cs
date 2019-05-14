﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.EntityFrameworkCore.Couchbase.Storage
{
    public class CouchbaseSqlQuery
    {
        public CouchbaseSqlQuery(string query, IReadOnlyList<SqlParameter> parameters)
        {
            Query = query;
            Parameters = parameters;
        }

        public string Query { get; }

        public IReadOnlyList<SqlParameter> Parameters { get; }
    }
}
