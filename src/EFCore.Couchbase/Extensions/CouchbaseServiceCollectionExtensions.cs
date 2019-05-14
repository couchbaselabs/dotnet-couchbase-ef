// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Couchbase.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.Couchbase.Infrastructure;
using Microsoft.EntityFrameworkCore.Couchbase.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Couchbase.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Couchbase.Query.ExpressionVisitors.Internal;
using Microsoft.EntityFrameworkCore.Couchbase.Query.Internal;
using Microsoft.EntityFrameworkCore.Couchbase.Query.Pipeline;
using Microsoft.EntityFrameworkCore.Couchbase.Query.Sql;
using Microsoft.EntityFrameworkCore.Couchbase.Query.Sql.Internal;
using Microsoft.EntityFrameworkCore.Couchbase.Storage.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using Microsoft.EntityFrameworkCore.Query.Pipeline;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CouchbaseServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFrameworkCouchbase([NotNull] this IServiceCollection serviceCollection)
        {
            Check.NotNull(serviceCollection, nameof(serviceCollection));

            var builder = new EntityFrameworkServicesBuilder(serviceCollection)
                .TryAdd<LoggingDefinitions, CouchbaseLoggingDefinitions>()
                .TryAdd<IDatabaseProvider, DatabaseProvider<CouchbaseOptionsExtension>>()
                .TryAdd<IDatabase, CouchbaseDatabaseWrapper>()
                .TryAdd<IExecutionStrategyFactory, CouchbaseExecutionStrategyFactory>()
                .TryAdd<IDbContextTransactionManager, CouchbaseTransactionManager>()
                .TryAdd<IModelCustomizer, CouchbaseModelCustomizer>()
                .TryAdd<IProviderConventionSetBuilder, CouchbaseConventionSetBuilder>()
                .TryAdd<IDatabaseCreator, CouchbaseDatabaseCreator>()
                .TryAdd<IQueryContextFactory, CouchbaseQueryContextFactory>()
                .TryAdd<IEntityQueryModelVisitorFactory, CouchbaseEntityQueryModelVisitorFactory>()
                .TryAdd<IEntityQueryableExpressionVisitorFactory, CouchbaseEntityQueryableExpressionVisitorFactory>()
                .TryAdd<IMemberAccessBindingExpressionVisitorFactory, CouchbaseMemberAccessBindingExpressionVisitorFactory>()
                .TryAdd<ITypeMappingSource, CouchbaseTypeMappingSource>()

                // New Query pipeline
                .TryAdd<IEntityQueryableTranslatorFactory, CouchbaseEntityQueryableTranslatorFactory>()
                .TryAdd<IQueryableMethodTranslatingExpressionVisitorFactory, CouchbaseQueryableMethodTranslatingExpressionVisitorFactory>()
                .TryAdd<IShapedQueryCompilingExpressionVisitorFactory, CouchbaseShapedQueryCompilingExpressionVisitorFactory>()

                .TryAddProviderSpecificServices(
                    b => b
                        .TryAddScoped<CouchbaseClientWrapper, CouchbaseClientWrapper>()
                        .TryAddScoped<ISqlGeneratorFactory, CouchbaseSqlGeneratorFactory>()
                );

            builder.TryAddCoreServices();

            return serviceCollection;
        }
    }
}
