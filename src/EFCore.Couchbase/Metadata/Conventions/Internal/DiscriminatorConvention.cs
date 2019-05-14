// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Couchbase.ValueGeneration.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Couchbase.Metadata.Conventions.Internal
{
    public class DiscriminatorConvention : IEntityTypeAddedConvention, IBaseTypeChangedConvention
    {
        public InternalEntityTypeBuilder Apply(InternalEntityTypeBuilder entityTypeBuilder)
        {
            if (entityTypeBuilder.Metadata.BaseType == null
                && !entityTypeBuilder.Metadata.GetDerivedTypes().Any())
            {
                ConfigureDiscriminator(entityTypeBuilder);
            }

            return entityTypeBuilder;
        }

        public bool Apply(InternalEntityTypeBuilder entityTypeBuilder, EntityType oldBaseType)
        {
            var entityType = entityTypeBuilder.Metadata;
            if (entityType.BaseType == null)
            {
                ConfigureDiscriminator(entityTypeBuilder);
                SetDefaultDiscriminatorValues(entityType.GetDerivedTypes());
            }
            else
            {
                if (entityType.BaseType.Couchbase().DiscriminatorProperty == null)
                {
                    ConfigureDiscriminator(entityType.BaseType.Builder);
                }

                entityType.Couchbase().DiscriminatorProperty = null;
                SetDefaultDiscriminatorValues(entityType.GetDerivedTypesInclusive());
            }

            return true;
        }

        private static void ConfigureDiscriminator(InternalEntityTypeBuilder entityTypeBuilder)
        {
            var propertyBuilder = entityTypeBuilder.Property(typeof(string), "Discriminator", ConfigurationSource.Convention);
            propertyBuilder.IsRequired(true, ConfigurationSource.Convention);
            propertyBuilder.AfterSave(PropertySaveBehavior.Throw, ConfigurationSource.Convention);
            propertyBuilder.HasValueGenerator(
                (_, et) => new DiscriminatorValueGenerator(et.Couchbase().DiscriminatorValue),
                ConfigurationSource.Convention);

            var entityType = entityTypeBuilder.Metadata;

            entityType.Couchbase().DiscriminatorProperty = propertyBuilder.Metadata;
            entityType.Couchbase().DiscriminatorValue = entityType.ShortName();
        }

        private static void SetDefaultDiscriminatorValues(IEnumerable<EntityType> entityTypes)
        {
            foreach (var entityType in entityTypes)
            {
                entityType.Couchbase().DiscriminatorValue = entityType.ShortName();
            }
        }
    }
}
