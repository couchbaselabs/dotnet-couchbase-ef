// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Couchbase.Infrastructure;
using Microsoft.EntityFrameworkCore.Couchbase.Storage.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.EntityFrameworkCore.Couchbase.TestUtilities
{
    public class CouchbaseTestStore : TestStore
    {
        private readonly TestStoreContext _storeContext;
        private readonly string _dataFilePath;

        public static CouchbaseTestStore Create(string name, Action<CouchbaseContextOptionsBuilder> extensionConfiguration = null) => new CouchbaseTestStore(name, shared: false, extensionConfiguration: extensionConfiguration);

        public static CouchbaseTestStore CreateInitialized(string name, Action<CouchbaseContextOptionsBuilder> extensionConfiguration = null)
            => (CouchbaseTestStore)Create(name, extensionConfiguration).Initialize(null, (Func<DbContext>)null, null, null);

        public static CouchbaseTestStore GetOrCreate(string name) => new CouchbaseTestStore(name);

        public static CouchbaseTestStore GetOrCreate(string name, string dataFilePath)
            => new CouchbaseTestStore(name, dataFilePath: dataFilePath);

        private CouchbaseTestStore(
            string name, bool shared = true, string dataFilePath = null, Action<CouchbaseContextOptionsBuilder> extensionConfiguration = null)
            : base(name, shared)
        {
            ConnectionUri = TestEnvironment.DefaultConnection;
            AuthToken = TestEnvironment.AuthToken;

            _storeContext = new TestStoreContext(this, extensionConfiguration);

            if (dataFilePath != null)
            {
                _dataFilePath = Path.Combine(
                    Path.GetDirectoryName(typeof(CouchbaseTestStore).GetTypeInfo().Assembly.Location),
                    dataFilePath);
            }
        }

        public string ConnectionUri { get; }
        public string AuthToken { get; }

        public override DbContextOptionsBuilder AddProviderOptions(DbContextOptionsBuilder builder)
            => builder.UseCouchbase(
                ConnectionUri,
                AuthToken,
                Name);

        protected override void Initialize(Func<DbContext> createContext, Action<DbContext> seed, Action<DbContext> clean)
        {
            if (_dataFilePath == null)
            {
                base.Initialize(createContext ?? (() => _storeContext), seed, clean);
            }
            else
            {
                using (var context = createContext())
                {
                    CreateFromFile(context).GetAwaiter().GetResult();
                }
            }
        }

        private async Task CreateFromFile(DbContext context)
        {
            if (await context.Database.EnsureCreatedAsync())
            {
                var CouchbaseClient = context.GetService<CouchbaseClientWrapper>();
                var serializer = new JsonSerializer();
                using (var fs = new FileStream(_dataFilePath, FileMode.Open, FileAccess.Read))
                using (var sr = new StreamReader(fs))
                using (var reader = new JsonTextReader(sr))
                {
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.StartArray)
                        {
                            NextEntityType:
                            while (reader.Read())
                            {
                                if (reader.TokenType == JsonToken.StartObject)
                                {
                                    string entityName = null;
                                    while (reader.Read())
                                    {
                                        if (reader.TokenType == JsonToken.PropertyName)
                                        {
                                            switch (reader.Value)
                                            {
                                                case "Name":
                                                    reader.Read();
                                                    entityName = (string)reader.Value;
                                                    break;
                                                case "Data":
                                                    while (reader.Read())
                                                    {
                                                        if (reader.TokenType == JsonToken.StartObject)
                                                        {
                                                            var document = serializer.Deserialize<JObject>(reader);

                                                            document["id"] = $"{entityName}|{document["id"]}";
                                                            document["Discriminator"] = entityName;
                                                            document["__partitionKey"] = "0";

                                                            await CouchbaseClient.CreateItemAsync("NorthwindContext", document);
                                                        }
                                                        else if (reader.TokenType == JsonToken.EndObject)
                                                        {
                                                            goto NextEntityType;
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void Clean(DbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        public override void Dispose()
        {
            if (_dataFilePath == null)
            {
                _storeContext.Database.EnsureDeleted();
            }

            _storeContext.Dispose();
            base.Dispose();
        }

        private class TestStoreContext : DbContext
        {
            private readonly CouchbaseTestStore _testStore;
            private readonly Action<CouchbaseContextOptionsBuilder> _extensionConfiguration;

            public TestStoreContext(CouchbaseTestStore testStore,
                Action<CouchbaseContextOptionsBuilder> extensionConfiguration)
            {
                _testStore = testStore;
                _extensionConfiguration = extensionConfiguration;
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                var extensionConfiguration = _extensionConfiguration ?? (_ => { });
                optionsBuilder.UseCouchbase(_testStore.ConnectionUri, _testStore.AuthToken, _testStore.Name, extensionConfiguration);
            }
        }
    }
}
