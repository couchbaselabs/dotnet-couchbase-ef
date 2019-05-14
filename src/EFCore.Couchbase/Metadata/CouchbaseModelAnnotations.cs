// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Couchbase.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Couchbase.Metadata
{
    public class CouchbaseModelAnnotations : ICouchbaseModelAnnotations
    {
        public CouchbaseModelAnnotations(IModel model)
            : this(new CouchbaseAnnotations(model))
        {
        }

        protected CouchbaseModelAnnotations(CouchbaseAnnotations annotations) => Annotations = annotations;

        protected virtual CouchbaseAnnotations Annotations { get; }

        protected virtual IModel Model => (IModel)Annotations.Metadata;

        public virtual string DefaultContainerName
        {
            get => (string)Annotations.Metadata[CouchbaseAnnotationNames.ContainerName];

            [param: CanBeNull] set => SetDefaultContainerName(value);
        }

        protected virtual bool SetDefaultContainerName([CanBeNull] string value)
            => Annotations.SetAnnotation(
                CouchbaseAnnotationNames.ContainerName,
                Check.NullButNotEmpty(value, nameof(value)));
    }
}
