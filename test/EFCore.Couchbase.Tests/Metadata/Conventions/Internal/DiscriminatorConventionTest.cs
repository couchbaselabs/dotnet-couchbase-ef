// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.EntityFrameworkCore.Couchbase.TestUtilities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Couchbase.Metadata.Conventions.Internal
{
    public class DiscriminatorConventionTest
    {
        [Fact]
        public void Creates_discriminator_property_and_sets_discriminator_value_for_entityType()
        {
            var entityTypeBuilder = CreateInternalEntityTypeBuilder<Entity>();
            var discriminatorProperty = entityTypeBuilder.Metadata.Couchbase().DiscriminatorProperty;

            Assert.NotNull(discriminatorProperty);
            Assert.Equal("Discriminator", discriminatorProperty.Name);
            Assert.Equal(typeof(string), discriminatorProperty.ClrType);
            Assert.Equal(typeof(Entity).Name, entityTypeBuilder.Metadata.Couchbase().DiscriminatorValue);
        }

        [Fact]
        public void Sets_discriminator_for_two_level_hierarchy()
        {
            var entityTypeBuilder = CreateInternalEntityTypeBuilder<Entity>();

            var baseTypeBuilder = entityTypeBuilder.ModelBuilder.Entity(typeof(EntityBase), ConfigurationSource.Explicit);
            var discriminator = entityTypeBuilder.Metadata.Couchbase().DiscriminatorProperty;

            Assert.NotNull(discriminator);
            Assert.Same(discriminator, baseTypeBuilder.Metadata.Couchbase().DiscriminatorProperty);
            Assert.Equal(typeof(EntityBase).Name, baseTypeBuilder.Metadata.Couchbase().DiscriminatorValue);
            Assert.Equal(typeof(Entity).Name, entityTypeBuilder.Metadata.Couchbase().DiscriminatorValue);

            Assert.NotNull(entityTypeBuilder.HasBaseType((Type)null, ConfigurationSource.DataAnnotation));
            Assert.NotSame(
                baseTypeBuilder.Metadata.Couchbase().DiscriminatorProperty,
                entityTypeBuilder.Metadata.Couchbase().DiscriminatorProperty);
            Assert.Equal(typeof(EntityBase).Name, baseTypeBuilder.Metadata.Couchbase().DiscriminatorValue);
            Assert.Equal(typeof(Entity).Name, entityTypeBuilder.Metadata.Couchbase().DiscriminatorValue);
        }

        [Fact]
        public void Sets_discriminator_for_three_level_hierarchy()
        {
            var entityTypeBuilder = CreateInternalEntityTypeBuilder<Entity>();
            var baseTypeBuilder = entityTypeBuilder.ModelBuilder.Entity(typeof(EntityBase), ConfigurationSource.Explicit);
            var derivedTypeBuilder = entityTypeBuilder.ModelBuilder.Entity(typeof(DerivedEntity), ConfigurationSource.Explicit);

            var baseDiscriminator = baseTypeBuilder.Metadata.Couchbase().DiscriminatorProperty;
            var discriminator = entityTypeBuilder.Metadata.Couchbase().DiscriminatorProperty;
            var derivedDiscriminator = derivedTypeBuilder.Metadata.Couchbase().DiscriminatorProperty;

            Assert.NotNull(discriminator);
            Assert.Same(discriminator, baseDiscriminator);
            Assert.Same(discriminator, derivedDiscriminator);
            Assert.Equal(typeof(EntityBase).Name, baseTypeBuilder.Metadata.Couchbase().DiscriminatorValue);
            Assert.Equal(typeof(Entity).Name, entityTypeBuilder.Metadata.Couchbase().DiscriminatorValue);
            Assert.Equal(typeof(DerivedEntity).Name, derivedTypeBuilder.Metadata.Couchbase().DiscriminatorValue);

            Assert.NotNull(entityTypeBuilder.HasBaseType((Type)null, ConfigurationSource.DataAnnotation));
            Assert.NotSame(
                baseTypeBuilder.Metadata.Couchbase().DiscriminatorProperty,
                entityTypeBuilder.Metadata.Couchbase().DiscriminatorProperty);
            Assert.Same(
                entityTypeBuilder.Metadata.Couchbase().DiscriminatorProperty,
                derivedTypeBuilder.Metadata.Couchbase().DiscriminatorProperty);
            Assert.Equal(typeof(EntityBase).Name, baseTypeBuilder.Metadata.Couchbase().DiscriminatorValue);
            Assert.Equal(typeof(Entity).Name, entityTypeBuilder.Metadata.Couchbase().DiscriminatorValue);
            Assert.Equal(typeof(DerivedEntity).Name, derivedTypeBuilder.Metadata.Couchbase().DiscriminatorValue);
        }

        private class EntityBase
        {
        }

        private class Entity : EntityBase
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        private class DerivedEntity : Entity
        {
        }

        private static InternalEntityTypeBuilder CreateInternalEntityTypeBuilder<T>()
        {
            var modelBuilder = CouchbaseTestHelpers.Instance.CreateConventionBuilder().GetInfrastructure();

            return modelBuilder.Entity(typeof(T), ConfigurationSource.Explicit);
        }
    }
}
