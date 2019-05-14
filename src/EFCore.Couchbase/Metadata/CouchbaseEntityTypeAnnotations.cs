﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Couchbase.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Couchbase.Metadata
{
    public class CouchbaseEntityTypeAnnotations : ICouchbaseEntityTypeAnnotations
    {
        public CouchbaseEntityTypeAnnotations(IEntityType entityType)
            : this(new CouchbaseAnnotations(entityType))
        {
        }

        protected CouchbaseEntityTypeAnnotations(CouchbaseAnnotations annotations) => Annotations = annotations;

        protected virtual CouchbaseAnnotations Annotations { get; }

        protected virtual IEntityType EntityType => (IEntityType)Annotations.Metadata;

        protected virtual CouchbaseModelAnnotations GetAnnotations(IModel model)
            => new CouchbaseModelAnnotations(model);

        protected virtual CouchbaseEntityTypeAnnotations GetAnnotations([NotNull] IEntityType entityType)
            => new CouchbaseEntityTypeAnnotations(entityType);

        public virtual string ContainerName
        {
            get => EntityType.BaseType != null
                ? GetAnnotations(EntityType.RootType()).ContainerName
                : ((string)Annotations.Metadata[CouchbaseAnnotationNames.ContainerName])
                  ?? GetDefaultContainerName();

            [param: CanBeNull] set => SetContainerName(value);
        }

        private string GetDefaultContainerName() => GetAnnotations(EntityType.Model).DefaultContainerName
                                                    ?? EntityType.ShortName();

        protected virtual bool SetContainerName([CanBeNull] string value)
            => Annotations.SetAnnotation(
                CouchbaseAnnotationNames.ContainerName,
                Check.NullButNotEmpty(value, nameof(value)));

        public virtual IProperty DiscriminatorProperty
        {
            get
            {
                if (EntityType.BaseType != null)
                {
                    return GetAnnotations(EntityType.RootType()).DiscriminatorProperty;
                }

                var propertyName = (string)Annotations.Metadata[CouchbaseAnnotationNames.DiscriminatorProperty];

                return propertyName == null ? null : EntityType.FindProperty(propertyName);
            }
            [param: CanBeNull] set => SetDiscriminatorProperty(value);
        }

        protected virtual bool SetDiscriminatorProperty([CanBeNull] IProperty value)
            => SetDiscriminatorProperty(value, DiscriminatorProperty?.ClrType);

        protected virtual bool SetDiscriminatorProperty([CanBeNull] IProperty value, [CanBeNull] Type oldDiscriminatorType)
        {
            if (value != null)
            {
                if (EntityType != EntityType.RootType())
                {
                    // TODO: Throw an exception
                    //throw new InvalidOperationException(
                    //    RelationalStrings.DiscriminatorPropertyMustBeOnRoot(EntityType.DisplayName()));
                }

                if (value.DeclaringEntityType != EntityType)
                {
                    // TODO: Throw an exception
                    //throw new InvalidOperationException(
                    //    RelationalStrings.DiscriminatorPropertyNotFound(value.Name, EntityType.DisplayName()));
                }
            }

            if (value == null
                || value.ClrType != oldDiscriminatorType)
            {
                foreach (var derivedType in EntityType.GetDerivedTypesInclusive())
                {
                    GetAnnotations(derivedType).RemoveDiscriminatorValue();
                }
            }

            return Annotations.SetAnnotation(
                CouchbaseAnnotationNames.DiscriminatorProperty,
                value?.Name);
        }

        protected virtual bool RemoveDiscriminatorValue()
            => Annotations.RemoveAnnotation(CouchbaseAnnotationNames.DiscriminatorValue);

        public virtual object DiscriminatorValue
        {
            get => Annotations.Metadata[CouchbaseAnnotationNames.DiscriminatorValue];
            [param: CanBeNull] set => SetDiscriminatorValue(value);
        }

        protected virtual bool SetDiscriminatorValue([CanBeNull] object value)
        {
            if (value != null
                && DiscriminatorProperty == null)
            {
                // TODO: Throw an exception
                //throw new InvalidOperationException(
                //    RelationalStrings.NoDiscriminatorForValue(EntityType.DisplayName(), EntityType.RootType().DisplayName()));
            }

            if (value != null
                && !DiscriminatorProperty.ClrType.GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo()))
            {
                // TODO: Throw an exception
                //throw new InvalidOperationException(
                //    RelationalStrings.DiscriminatorValueIncompatible(
                //        value, DiscriminatorProperty.Name, DiscriminatorProperty.ClrType));
            }

            return Annotations.SetAnnotation(CouchbaseAnnotationNames.DiscriminatorValue, value);
        }

        public string ContainingPropertyName
        {
            get => Annotations.Metadata[CouchbaseAnnotationNames.PropertyName] as string
                ?? GetDefaultContainingPropertyName();

            [param: CanBeNull] set => SetPropertyName(value);
        }

        private string GetDefaultContainingPropertyName()
            => EntityType.FindOwnership()?.PrincipalToDependent.Name;

        protected virtual bool SetPropertyName([CanBeNull] string value)
            => Annotations.SetAnnotation(
                CouchbaseAnnotationNames.PropertyName,
                Check.NullButNotEmpty(value, nameof(value)));
    }
}
