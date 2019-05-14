// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Couchbase.Internal;
using Microsoft.EntityFrameworkCore.Couchbase.TestUtilities;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.TestModels.TransportationModel;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.EntityFrameworkCore.Couchbase
{
    public class NestedDocumentsTest
    {
        public NestedDocumentsTest(ITestOutputHelper testOutputHelper)
        {
            TestSqlLoggerFactory = (TestSqlLoggerFactory)TestStoreFactory.CreateListLoggerFactory(_ => true);
            //TestSqlLoggerFactory.SetTestOutputHelper(testOutputHelper);
        }

        // #13579
        // [Fact]
        public virtual void Can_update_dependents()
        {
            using (CreateTestStore())
            {
                Operator firstOperator;
                Engine firstEngine;
                using (var context = CreateContext())
                {
                    firstOperator = context.Set<Vehicle>().Select(v => v.Operator).OrderBy(o => o.VehicleName).First();
                    firstOperator.Name += "1";
                    firstEngine = context.Set<PoweredVehicle>().Select(v => v.Engine).OrderBy(o => o.VehicleName).First();
                    firstEngine.Description += "1";

                    context.SaveChanges();
                }

                using (var context = CreateContext())
                {
                    Assert.Equal(
                        firstOperator.Name,
                        context.Set<Vehicle>().Select(v => v.Operator).OrderBy(o => o.VehicleName).First().Name);
                    Assert.Equal(
                        firstEngine.Description,
                        context.Set<PoweredVehicle>().Select(v => v.Engine).OrderBy(o => o.VehicleName).First().Description);
                }
            }
        }

        // #13579
        // [Fact]
        public virtual void Can_update_owner_with_dependents()
        {
            using (CreateTestStore())
            {
                Operator firstOperator;
                using (var context = CreateContext())
                {
                    firstOperator = context.Set<Vehicle>().OrderBy(o => o.Operator.VehicleName).First().Operator;
                    firstOperator.Name += "1";

                    context.SaveChanges();
                }

                using (var context = CreateContext())
                {
                    Assert.Equal(
                        firstOperator.Name,
                        context.Set<Vehicle>().OrderBy(o => o.Operator.VehicleName).First().Operator.Name);
                }
            }
        }

        [Fact]
        public virtual void Can_add_collection_dependent_to_owner()
        {
            using (CreateTestStore())
            {
                Address existingAddress1Person2;
                Address existingAddress1Person3;
                Address existingAddress2Person3;
                Address addedAddress1;
                Address addedAddress2;
                Address addedAddress3;
                using (var context = CreateContext())
                {
                    context.Add(new Person { Id = 1 });
                    existingAddress1Person2 = new Address { Street = "Second", City = "Village" };
                    context.Add(new Person { Id = 2, Addresses = new[] { existingAddress1Person2 } });
                    existingAddress1Person3 = new Address { Street = "First", City = "City" };
                    existingAddress2Person3 = new Address { Street = "Second", City = "City" };
                    context.Add(new Person { Id = 3, Addresses = new[] { existingAddress1Person3, existingAddress2Person3 } });

                    context.SaveChanges();
                }

                using (var context = CreateContext())
                {
                    var people = context.Set<Person>().ToList();
                    addedAddress1 = new Address { Street = "First", City = "Town" };
                    people[0].Addresses.Add(addedAddress1);

                    addedAddress2 = new Address { Street = "Another", City = "Village" };
                    people[1].Addresses.Add(addedAddress2);

                    // Remove when issues #13578 and #13579 are fixed
                    var existingEntry = context.Attach(people[1].Addresses.First());

                    var secondPersonEntry = context.Entry(people[1]);
                    var json = secondPersonEntry.Property<JObject>("__jObject").CurrentValue;

                    var adresses = (JArray)json["Stored Addresses"];
                    var jsonAddress = (JObject)adresses[0];
                    Assert.Equal("Second", jsonAddress[nameof(Address.Street)]);
                    jsonAddress["unmappedId"] = 2;

                    secondPersonEntry.Property<JObject>("__jObject").CurrentValue = json;

                    addedAddress3 = new Address { Street = "Another", City = "City" };
                    var existingLastAddress = people[2].Addresses.Last();
                    people[2].Addresses.Remove(existingLastAddress);
                    people[2].Addresses.Add(addedAddress3);
                    people[2].Addresses.Add(existingLastAddress);

                    // Remove when issues #13578 and #13579 are fixed
                    context.Attach(people[2].Addresses.First());
                    context.Attach(existingLastAddress);

                    context.SaveChanges();
                }

                using (var context = CreateContext())
                {
                    var people = context.Set<Person>().OrderBy(o => o.Id).ToList();
                    var addresses = people[0].Addresses.ToList();
                    Assert.Equal(addedAddress1.Street, addresses.Single().Street);
                    Assert.Equal(addedAddress1.City, addresses.Single().City);

                    addresses = people[1].Addresses.ToList();
                    Assert.Equal(2, addresses.Count);

                    Assert.Equal(existingAddress1Person2.Street, addresses[0].Street);
                    Assert.Equal(existingAddress1Person2.City, addresses[0].City);

                    Assert.Equal(addedAddress2.Street, addresses[1].Street);
                    Assert.Equal(addedAddress2.City, addresses[1].City);

                    var json = context.Entry(people[1]).Property<JObject>("__jObject").CurrentValue;
                    var jsonAddress = (JObject)((JArray)json["Stored Addresses"])[0];
                    Assert.Equal("Second", jsonAddress[nameof(Address.Street)]);
                    //Assert.Equal(2, jsonAddress["unmappedId"]);

                    addresses = people[2].Addresses.ToList();
                    Assert.Equal(3, addresses.Count);

                    Assert.Equal(existingAddress1Person3.Street, addresses[0].Street);
                    Assert.Equal(existingAddress1Person3.City, addresses[0].City);

                    Assert.Equal(addedAddress3.Street, addresses[1].Street);
                    Assert.Equal(addedAddress3.City, addresses[1].City);

                    Assert.Equal(existingAddress2Person3.Street, addresses[2].Street);
                    Assert.Equal(existingAddress2Person3.City, addresses[2].City);
                }
            }
        }

        // #13559
        //[Fact]
        public virtual void Can_update_just_dependents()
        {
            using (CreateTestStore())
            {
                Operator firstOperator;
                Engine firstEngine;
                using (var context = CreateContext())
                {
                    firstOperator = context.Set<Operator>().OrderBy(o => o.VehicleName).First();
                    firstOperator.Name += "1";
                    firstEngine = context.Set<Engine>().OrderBy(o => o.VehicleName).First();
                    firstEngine.Description += "1";

                    context.SaveChanges();
                }

                using (var context = CreateContext())
                {
                    Assert.Equal(firstOperator.Name, context.Set<Operator>().OrderBy(o => o.VehicleName).First().Name);
                    Assert.Equal(firstEngine.Description, context.Set<Engine>().OrderBy(o => o.VehicleName).First().Description);
                }
            }
        }

        [Fact]
        public virtual void Inserting_dependent_without_principal_throws()
        {
            using (CreateTestStore())
            {
                using (var context = CreateContext())
                {
                    context.Add(
                        new LicensedOperator
                        {
                            Name = "Jack Jackson",
                            LicenseType = "Class A CDC",
                            VehicleName = "Fuel transport"
                        });

                    Assert.Equal(
                        CouchbaseStrings.OrphanedNestedDocumentSensitive(
                            nameof(Operator), nameof(Vehicle), "{VehicleName: Fuel transport}"),
                        Assert.Throws<InvalidOperationException>(() => context.SaveChanges()).Message);
                }
            }
        }

        [Fact]
        public virtual void Can_change_nested_instance_non_derived()
        {
            using (CreateTestStore())
            {
                using (var context = CreateContext())
                {
                    var bike = context.Vehicles.Single(v => v.Name == "Trek Pro Fit Madone 6 Series");

                    bike.Operator = new Operator
                    {
                        Name = "Chris Horner"
                    };

                    context.ChangeTracker.DetectChanges();

                    bike.Operator = new LicensedOperator
                    {
                        Name = "repairman"
                    };

                    TestSqlLoggerFactory.Clear();
                    context.SaveChanges();
                }

                using (var context = CreateContext())
                {
                    var bike = context.Vehicles.Single(v => v.Name == "Trek Pro Fit Madone 6 Series");
                    Assert.Equal("repairman", bike.Operator.Name);
                }
            }
        }

        [Fact]
        public virtual void Can_change_principal_instance_non_derived()
        {
            using (CreateTestStore())
            {
                using (var context = CreateContext())
                {
                    var bike = context.Vehicles.Single(v => v.Name == "Trek Pro Fit Madone 6 Series");

                    var newBike = new Vehicle
                    {
                        Name = "Trek Pro Fit Madone 6 Series",
                        Operator = bike.Operator,
                        SeatingCapacity = 2
                    };

                    var oldEntry = context.Remove(bike);
                    var newEntry = context.Add(newBike);

                    TestSqlLoggerFactory.Clear();
                    context.SaveChanges();
                }

                using (var context = CreateContext())
                {
                    var bike = context.Vehicles.Single(v => v.Name == "Trek Pro Fit Madone 6 Series");

                    Assert.Equal(2, bike.SeatingCapacity);
                    Assert.NotNull(bike.Operator);
                }
            }
        }

        protected readonly string DatabaseName = "NestedDocumentsTest";
        protected TestStore TestStore { get; set; }
        protected ITestStoreFactory TestStoreFactory => CouchbaseTestStoreFactory.Instance;
        protected IServiceProvider ServiceProvider { get; set; }
        protected TestSqlLoggerFactory TestSqlLoggerFactory { get; }

        protected void AssertSql(params string[] expected)
            => TestSqlLoggerFactory.AssertBaseline(expected);

        protected void AssertContainsSql(params string[] expected)
            => TestSqlLoggerFactory.AssertBaseline(expected, assertOrder: false);

        protected TestStore CreateTestStore(Action<ModelBuilder> onModelCreating = null)
        {
            TestStore = TestStoreFactory.Create(DatabaseName);

            ServiceProvider = TestStoreFactory.AddProviderServices(new ServiceCollection())
                .AddSingleton(TestModelSource.GetFactory(onModelCreating ?? (_ => { })))
                .AddSingleton<ILoggerFactory>(TestSqlLoggerFactory)
                .BuildServiceProvider(validateScopes: true);

            TestStore.Initialize(ServiceProvider, CreateContext, c => ((TransportationContext)c).Seed());

            TestSqlLoggerFactory.Clear();

            return TestStore;
        }

        protected virtual DbContextOptionsBuilder AddOptions(DbContextOptionsBuilder builder)
            => builder
                .EnableSensitiveDataLogging()
                .ConfigureWarnings(
                    b => b.Default(WarningBehavior.Throw)
                        .Log(CoreEventId.SensitiveDataLoggingEnabledWarning)
                        .Log(CoreEventId.PossibleUnintendedReferenceComparisonWarning));

        protected virtual NestedTransportationContext CreateContext()
        {
            var options = AddOptions(TestStore.AddProviderOptions(new DbContextOptionsBuilder()))
                .UseInternalServiceProvider(ServiceProvider).Options;
            return new NestedTransportationContext(options);
        }

        protected class NestedTransportationContext : TransportationContext
        {
            public NestedTransportationContext(DbContextOptions options) : base(options)
            {
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Vehicle>(
                    eb =>
                    {
                        eb.HasKey(e => e.Name);
                        eb.OwnsOne(v => v.Operator);
                    });

                modelBuilder.Entity<Engine>(
                    eb =>
                    {
                        eb.HasKey(e => e.VehicleName);
                        eb.HasOne(e => e.Vehicle)
                            .WithOne(e => e.Engine)
                            .HasForeignKey<Engine>(e => e.VehicleName);
                    });

                modelBuilder.Entity<FuelTank>(
                    eb =>
                    {
                        eb.HasKey(e => e.VehicleName);
                        eb.HasOne(e => e.Engine)
                            .WithOne(e => e.FuelTank)
                            .HasForeignKey<FuelTank>(e => e.VehicleName)
                            .OnDelete(DeleteBehavior.Restrict);
                    });

                modelBuilder.Entity<ContinuousCombustionEngine>();
                modelBuilder.Entity<IntermittentCombustionEngine>();

                modelBuilder.Ignore<SolidFuelTank>();
                modelBuilder.Ignore<SolidRocket>();

                modelBuilder.Entity<Person>(
                    eb => eb.OwnsMany(v => v.Addresses, b =>
                    {
                        b.ToProperty("Stored Addresses");
                        b.HasKey(v => new { v.Street, v.City });
                    }));
            }
        }

        private class Person
        {
            public int Id { get; set; }
            public ICollection<Address> Addresses { get; set; }
        }

        public class Address
        {
            public string Street { get; set; }
            public string City { get; set; }
        }
    }
}
