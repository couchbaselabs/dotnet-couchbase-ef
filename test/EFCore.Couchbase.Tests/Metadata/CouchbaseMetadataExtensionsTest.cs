﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Xunit;

namespace Microsoft.EntityFrameworkCore
{
    public class CouchbaseMetadataExtensionsTest
    {
        [Fact]
        public void Can_get_and_set_collection_name()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            var entityType = modelBuilder
                .Entity<Customer>();

            Assert.Equal(nameof(Customer), entityType.Metadata.Couchbase().ContainerName);

            entityType.ToContainer("Customizer");
            Assert.Equal("Customizer", entityType.Metadata.Couchbase().ContainerName);

            entityType.ToContainer(null);
            Assert.Equal(nameof(Customer), entityType.Metadata.Couchbase().ContainerName);

            modelBuilder.HasDefaultContainerName("Unicorn");
            Assert.Equal("Unicorn", entityType.Metadata.Couchbase().ContainerName);
        }

        private class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Guid AlternateId { get; set; }
        }
    }
}
