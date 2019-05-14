// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Couchbase.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Couchbase.TestUtilities;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Microsoft.EntityFrameworkCore.TestUtilities.Xunit;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Couchbase
{
    public class CouchbaseEndToEndTest : IClassFixture<CouchbaseEndToEndTest.CouchbaseFixture>
    {
        private const string DatabaseName = "CouchbaseEndToEndTest";

        protected CouchbaseFixture Fixture { get; }

        public CouchbaseEndToEndTest(CouchbaseFixture fixture)
        {
            Fixture = fixture;
        }

        [ConditionalFact]
        public void Can_add_update_delete_end_to_end()
        {
            using (var testDatabase = CouchbaseTestStore.CreateInitialized(DatabaseName))
            {
                var options = Fixture.CreateOptions(testDatabase);

                var customer = new Customer { Id = 42, Name = "Theon" };

                using (var context = new CustomerContext(options))
                {
                    context.Database.EnsureCreated();

                    context.Add(customer);

                    context.SaveChanges();
                }

                using (var context = new CustomerContext(options))
                {
                    var customerFromStore = context.Set<Customer>().Single();

                    Assert.Equal(42, customerFromStore.Id);
                    Assert.Equal("Theon", customerFromStore.Name);

                    customerFromStore.Name = "Theon Greyjoy";

                    context.SaveChanges();
                }

                using (var context = new CustomerContext(options))
                {
                    var customerFromStore = context.Set<Customer>().Single();

                    Assert.Equal(42, customerFromStore.Id);
                    Assert.Equal("Theon Greyjoy", customerFromStore.Name);

                    context.Remove(customerFromStore);

                    context.SaveChanges();
                }

                using (var context = new CustomerContext(options))
                {
                    Assert.Equal(0, context.Set<Customer>().Count());
                }
            }
        }

        [ConditionalFact]
        public async Task Can_add_update_delete_end_to_end_async()
        {
            using (var testDatabase = CouchbaseTestStore.CreateInitialized(DatabaseName))
            {
                var options = Fixture.CreateOptions(testDatabase);

                var customer = new Customer { Id = 42, Name = "Theon" };

                using (var context = new CustomerContext(options))
                {
                    await context.Database.EnsureCreatedAsync();

                    context.Add(customer);

                    await context.SaveChangesAsync();
                }

                using (var context = new CustomerContext(options))
                {
                    var customerFromStore = context.Set<Customer>().Single();

                    Assert.Equal(42, customerFromStore.Id);
                    Assert.Equal("Theon", customerFromStore.Name);

                    customerFromStore.Name = "Theon Greyjoy";

                    await context.SaveChangesAsync();
                }

                using (var context = new CustomerContext(options))
                {
                    var customerFromStore = context.Set<Customer>().Single();

                    Assert.Equal(42, customerFromStore.Id);
                    Assert.Equal("Theon Greyjoy", customerFromStore.Name);

                    context.Remove(customerFromStore);

                    await context.SaveChangesAsync();
                }

                using (var context = new CustomerContext(options))
                {
                    Assert.Equal(0, await context.Set<Customer>().CountAsync());
                }
            }
        }

        private class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class CustomerContext : DbContext
        {
            public CustomerContext(DbContextOptions dbContextOptions)
                : base(dbContextOptions)
            {
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Customer>();
            }
        }

        [ConditionalFact]
        public async Task Can_add_update_delete_detached_entity_end_to_end_async()
        {
            using (var testDatabase = CouchbaseTestStore.CreateInitialized(DatabaseName))
            {
                var options = Fixture.CreateOptions(testDatabase);

                var customer = new Customer { Id = 42, Name = "Theon" };

                string storeId = null;
                using (var context = new CustomerContext(options))
                {
                    await context.Database.EnsureCreatedAsync();

                    var entry = context.Add(customer);

                    await context.SaveChangesAsync();

                    context.Add(customer);

                    storeId = entry.Property<string>("id").CurrentValue;
                }

                Assert.NotNull(storeId);

                using (var context = new CustomerContext(options))
                {
                    var customerFromStore = context.Set<Customer>().Single();

                    Assert.Equal(42, customerFromStore.Id);
                    Assert.Equal("Theon", customerFromStore.Name);
                }

                using (var context = new CustomerContext(options))
                {
                    customer.Name = "Theon Greyjoy";

                    var entry = context.Entry(customer);
                    entry.Property<string>("id").CurrentValue = storeId;

                    entry.State = EntityState.Modified;

                    await context.SaveChangesAsync();
                }

                using (var context = new CustomerContext(options))
                {
                    var customerFromStore = context.Set<Customer>().Single();

                    Assert.Equal(42, customerFromStore.Id);
                    Assert.Equal("Theon Greyjoy", customerFromStore.Name);
                }

                using (var context = new CustomerContext(options))
                {
                    var entry = context.Entry(customer);
                    entry.Property<string>("id").CurrentValue = storeId;
                    entry.State = EntityState.Deleted;

                    await context.SaveChangesAsync();
                }

                using (var context = new CustomerContext(options))
                {
                    Assert.Equal(0, context.Set<Customer>().Count());
                }
            }
        }

        [ConditionalFact]
        public void Can_update_unmapped_properties()
        {
            using (var testDatabase = CouchbaseTestStore.CreateInitialized(DatabaseName))
            {
                var options = Fixture.CreateOptions(testDatabase);

                var customer = new Customer { Id = 42, Name = "Theon" };

                using (var context = new ExtraCustomerContext(options))
                {
                    context.Database.EnsureCreated();

                    var entry = context.Add(customer);
                    entry.Property<string>("EMail").CurrentValue = "theon.g@winterfell.com";

                    context.SaveChanges();
                }

                using (var context = new CustomerContext(options))
                {
                    var customerFromStore = context.Set<Customer>().Single();

                    Assert.Equal(42, customerFromStore.Id);
                    Assert.Equal("Theon", customerFromStore.Name);

                    customerFromStore.Name = "Theon Greyjoy";

                    context.SaveChanges();
                }

                using (var context = new ExtraCustomerContext(options))
                {
                    var customerFromStore = context.Set<Customer>().Single();

                    Assert.Equal(42, customerFromStore.Id);
                    Assert.Equal("Theon Greyjoy", customerFromStore.Name);

                    var entry = context.Entry(customerFromStore);
                    Assert.Equal("theon.g@winterfell.com", entry.Property<string>("EMail").CurrentValue);

                    var json = entry.Property<JObject>("__jObject").CurrentValue;
                    Assert.Equal("theon.g@winterfell.com", json["e-mail"]);

                    context.Remove(customerFromStore);

                    context.SaveChanges();
                }
            }
        }

        public class ExtraCustomerContext : CustomerContext
        {
            public ExtraCustomerContext(DbContextOptions dbContextOptions)
                : base(dbContextOptions)
            {
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.HasDefaultContainerName(nameof(CustomerContext));
                modelBuilder.Entity<Customer>().Property<string>("EMail").ToProperty("e-mail");
            }
        }

        [ConditionalFact]
        public void Can_use_non_persisted_properties()
        {
            using (var testDatabase = CouchbaseTestStore.CreateInitialized(DatabaseName))
            {
                var options = Fixture.CreateOptions(testDatabase);

                var customer = new Customer { Id = 42, Name = "Theon" };

                using (var context = new UnmpappedCustomerContext(options))
                {
                    context.Database.EnsureCreated();

                    var entry = context.Add(customer);

                    context.SaveChanges();
                    Assert.Equal("Theon", customer.Name);
                }

                using (var context = new UnmpappedCustomerContext(options))
                {
                    var customerFromStore = context.Set<Customer>().Single();

                    Assert.Equal(42, customerFromStore.Id);
                    Assert.Null(customerFromStore.Name);

                    customerFromStore.Name = "Theon Greyjoy";

                    Assert.Equal(0, context.SaveChanges());
                }
            }
        }

        public class UnmpappedCustomerContext : CustomerContext
        {
            public UnmpappedCustomerContext(DbContextOptions dbContextOptions)
                : base(dbContextOptions)
            {
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Customer>().Property(c => c.Name).ToProperty("");
            }
        }

        [ConditionalFact]
        public async Task Using_a_conflicting_incompatible_id_throws()
        {
            using (var testDatabase = CouchbaseTestStore.CreateInitialized(DatabaseName))
            {
                var options = Fixture.CreateOptions(testDatabase);

                using (var context = new ConflictingIncompatibleIdContext(options))
                {
                    await Assert.ThrowsAnyAsync<Exception>(
                        async () =>
                        {
                            await context.Database.EnsureCreatedAsync();

                            context.Add(new ConflictingIncompatibleId { id = 42 });

                            await context.SaveChangesAsync();
                        });
                }
            }
        }

        private class ConflictingIncompatibleId
        {
            // ReSharper disable once InconsistentNaming
            public int id { get; set; }
            public string Name { get; set; }
        }

        public class ConflictingIncompatibleIdContext : DbContext
        {
            public ConflictingIncompatibleIdContext(DbContextOptions dbContextOptions)
                : base(dbContextOptions)
            {
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<ConflictingIncompatibleId>();
            }
        }

//        [ConditionalFact]
//        public void Should_not_throw_if_specified_region_is_right()
//        {
//            var regionName = CouchbaseRegions.AustraliaCentral;
//
//            using (var testDatabase = CouchbaseTestStore.CreateInitialized(DatabaseName, o => o.Region(regionName)))
//            {
//                var options = Fixture.CreateOptions(testDatabase);
//
//                var customer = new Customer { Id = 42, Name = "Theon" };
//
//                using (var context = new CustomerContext(options))
//                {
//                    context.Database.EnsureCreated();
//
//                    context.Add(customer);
//
//                    context.SaveChanges();
//                }
//            }
//        }

        [ConditionalFact]
        public void Should_throw_if_specified_region_is_wrong()
        {
            var regionName = "FakeRegion";

            Action a = () =>
            {
                using (var testDatabase = CouchbaseTestStore.CreateInitialized(DatabaseName, o => o.Region(regionName)))
                {
                    var options = Fixture.CreateOptions(testDatabase);

                    var customer = new Customer {Id = 42, Name = "Theon"};

                    using (var context = new CustomerContext(options))
                    {
                        context.Database.EnsureCreated();

                        context.Add(customer);

                        context.SaveChanges();
                    }
                }
            };

            var ex = Assert.Throws<ArgumentException>(a);
            Assert.Equal("Current location is not a valid Azure region.", ex.Message);
        }

        [ConditionalFact]
        public async Task Can_add_update_delete_end_to_end_with_conflicting_id()
        {
            using (var testDatabase = CouchbaseTestStore.CreateInitialized(DatabaseName))
            {
                var options = Fixture.CreateOptions(testDatabase);

                var entity = new ConflictingId { id = "42", Name = "Theon" };

                using (var context = new ConflictingIdContext(options))
                {
                    await context.Database.EnsureCreatedAsync();

                    context.Add(entity);

                    await context.SaveChangesAsync();
                }

                using (var context = new ConflictingIdContext(options))
                {
                    var entityFromStore = context.Set<ConflictingId>().Single();

                    Assert.Equal("42", entityFromStore.id);
                    Assert.Equal("Theon", entityFromStore.Name);
                }

                using (var context = new ConflictingIdContext(options))
                {
                    entity.Name = "Theon Greyjoy";

                    context.Update(entity);

                    await context.SaveChangesAsync();
                }

                using (var context = new ConflictingIdContext(options))
                {
                    var entityFromStore = context.Set<ConflictingId>().Single();

                    Assert.Equal("42", entityFromStore.id);
                    Assert.Equal("Theon Greyjoy", entityFromStore.Name);
                }

                using (var context = new ConflictingIdContext(options))
                {
                    context.Remove(entity);

                    await context.SaveChangesAsync();
                }

                using (var context = new ConflictingIdContext(options))
                {
                    Assert.Equal(0, context.Set<ConflictingId>().Count());
                }
            }
        }

        private class ConflictingId
        {
            public string id { get; set; }
            public string Name { get; set; }
        }

        public class ConflictingIdContext : DbContext
        {
            public ConflictingIdContext(DbContextOptions dbContextOptions)
                : base(dbContextOptions)
            {
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<ConflictingId>();
            }
        }

        public class CouchbaseFixture : ServiceProviderFixtureBase
        {
            protected override ITestStoreFactory TestStoreFactory => CouchbaseTestStoreFactory.Instance;
        }
    }
}
