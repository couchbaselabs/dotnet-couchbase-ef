// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Couchbase.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Couchbase.Metadata
{
    public class CouchbasePropertyAnnotations : ICouchbasePropertyAnnotations
    {
        public CouchbasePropertyAnnotations(IProperty property)
            : this(new CouchbaseAnnotations(property))
        {
        }

        protected CouchbasePropertyAnnotations(CouchbaseAnnotations annotations) => Annotations = annotations;

        protected virtual CouchbaseAnnotations Annotations { get; }

        protected virtual IProperty Property => (IProperty)Annotations.Metadata;

        public virtual string PropertyName
        {
            get => ((string)Annotations.Metadata[CouchbaseAnnotationNames.PropertyName])
                   ?? Property.Name;

            [param: CanBeNull] set => SetPropertyName(value);
        }

        protected virtual bool SetPropertyName([CanBeNull] string value)
            => Annotations.SetAnnotation(
                CouchbaseAnnotationNames.PropertyName,
                value);
    }
}
