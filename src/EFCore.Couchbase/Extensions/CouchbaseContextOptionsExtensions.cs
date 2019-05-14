// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Couchbase.Infrastructure;
using Microsoft.EntityFrameworkCore.Couchbase.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore
{
    public static class CouchbaseContextOptionsExtensions
    {
        public static DbContextOptionsBuilder<TContext> UseCouchbase<TContext>(
            [NotNull] this DbContextOptionsBuilder<TContext> optionsBuilder,
            [NotNull] string serviceEndPoint,
            [NotNull] string authKeyOrResourceToken,
            [NotNull] string databaseName,
            [CanBeNull] Action<CouchbaseContextOptionsBuilder> CouchbaseOptionsAction = null)
            where TContext : DbContext
            => (DbContextOptionsBuilder<TContext>)UseCouchbase(
                (DbContextOptionsBuilder)optionsBuilder,
                serviceEndPoint,
                authKeyOrResourceToken,
                databaseName,
                CouchbaseOptionsAction);

        public static DbContextOptionsBuilder UseCouchbase(
            [NotNull] this DbContextOptionsBuilder optionsBuilder,
            [NotNull] string serviceEndPoint,
            [NotNull] string authKeyOrResourceToken,
            [NotNull] string databaseName,
            [CanBeNull] Action<CouchbaseContextOptionsBuilder> CouchbaseOptionsAction = null)
        {
            Check.NotNull(optionsBuilder, nameof(optionsBuilder));
            Check.NotNull(serviceEndPoint, nameof(serviceEndPoint));
            Check.NotEmpty(authKeyOrResourceToken, nameof(authKeyOrResourceToken));
            Check.NotEmpty(databaseName, nameof(databaseName));

            var extension = optionsBuilder.Options.FindExtension<CouchbaseOptionsExtension>()
                            ?? new CouchbaseOptionsExtension();

            extension = extension
                .WithServiceEndPoint(serviceEndPoint)
                .WithAuthKeyOrResourceToken(authKeyOrResourceToken)
                .WithDatabaseName(databaseName);

            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

            CouchbaseOptionsAction?.Invoke(new CouchbaseContextOptionsBuilder(optionsBuilder));

            return optionsBuilder;
        }
    }
}
