// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace Microsoft.EntityFrameworkCore.Couchbase.Storage.Internal
{
    public class CouchbaseDatabaseCreator : IDatabaseCreator
    {
        private readonly CouchbaseClientWrapper _CouchbaseClient;
        private readonly IModel _model;
        private readonly IUpdateAdapterFactory _updateAdapterFactory;
        private readonly IDatabase _database;

        public CouchbaseDatabaseCreator(
            CouchbaseClientWrapper CouchbaseClient,
            IModel model,
            IUpdateAdapterFactory updateAdapterFactory,
            IDatabase database)
        {
            _CouchbaseClient = CouchbaseClient;
            _model = model;
            _updateAdapterFactory = updateAdapterFactory;
            _database = database;
        }

        public bool EnsureCreated()
        {
            var created = _CouchbaseClient.CreateDatabaseIfNotExists();
            foreach (var entityType in _model.GetEntityTypes())
            {
                created |= _CouchbaseClient.CreateContainerIfNotExists(entityType.Couchbase().ContainerName, "__partitionKey");
            }

            if (created)
            {
                var updateAdapter = _updateAdapterFactory.Create();
                foreach (var entityType in _model.GetEntityTypes())
                {
                    foreach (var targetSeed in entityType.GetSeedData())
                    {
                        var entry = updateAdapter.CreateEntry(targetSeed, entityType);
                        entry.EntityState = EntityState.Added;
                    }
                }

                _database.SaveChanges(updateAdapter.GetEntriesToSave());
            }

            return created;
        }

        public async Task<bool> EnsureCreatedAsync(CancellationToken cancellationToken = default)
        {
            var created = await _CouchbaseClient.CreateDatabaseIfNotExistsAsync(cancellationToken);
            foreach (var entityType in _model.GetEntityTypes())
            {
                created |= await _CouchbaseClient.CreateContainerIfNotExistsAsync(entityType.Couchbase().ContainerName, "__partitionKey", cancellationToken);
            }

            if (created)
            {
                var updateAdapter = _updateAdapterFactory.Create();
                foreach (var entityType in _model.GetEntityTypes())
                {
                    foreach (var targetSeed in entityType.GetSeedData())
                    {
                        var entry = updateAdapter.CreateEntry(targetSeed, entityType);
                        entry.EntityState = EntityState.Added;
                    }
                }

                await _database.SaveChangesAsync(updateAdapter.GetEntriesToSave(), cancellationToken);
            }

            return created;
        }

        public bool EnsureDeleted() => _CouchbaseClient.DeleteDatabase();

        public Task<bool> EnsureDeletedAsync(CancellationToken cancellationToken = default)
            => _CouchbaseClient.DeleteDatabaseAsync(cancellationToken);

        public virtual bool CanConnect()
            => throw new NotImplementedException();

        public virtual Task<bool> CanConnectAsync(CancellationToken cancellationToken = default)
            => throw new NotImplementedException();
    }
}
