// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Couchbase.Metadata;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    ///     Couchbase-specific extension methods for metadata.
    /// </summary>
    public static class CouchbaseMetadataExtensions
    {
        /// <summary>
        ///     Gets the Couchbase-specific metadata for a model.
        /// </summary>
        /// <param name="model"> The model to get metadata for. </param>
        /// <returns> The Couchbase-specific metadata for the model. </returns>
        public static ICouchbaseModelAnnotations Couchbase(this IModel model)
            => new CouchbaseModelAnnotations(model);

        /// <summary>
        ///     Gets the Couchbase-specific metadata for a model.
        /// </summary>
        /// <param name="model"> The model to get metadata for. </param>
        /// <returns> The Couchbase-specific metadata for the model. </returns>
        public static CouchbaseModelAnnotations Couchbase(this IMutableModel model)
            => (CouchbaseModelAnnotations)Couchbase((IModel)model);

        /// <summary>
        ///     Gets the Couchbase-specific metadata for an entity type.
        /// </summary>
        /// <param name="entityType"> The entity type to get metadata for. </param>
        /// <returns> The Couchbase-specific metadata for the entity type. </returns>
        public static ICouchbaseEntityTypeAnnotations Couchbase(this IEntityType entityType)
            => new CouchbaseEntityTypeAnnotations(entityType);

        /// <summary>
        ///     Gets the Couchbase-specific metadata for an entity type.
        /// </summary>
        /// <param name="entityType"> The entity type to get metadata for. </param>
        /// <returns> The Couchbase-specific metadata for the entity type. </returns>
        public static CouchbaseEntityTypeAnnotations Couchbase(this IMutableEntityType entityType)
            => (CouchbaseEntityTypeAnnotations)Couchbase((IEntityType)entityType);

        /// <summary>
        ///     Gets the Couchbase-specific metadata for a property.
        /// </summary>
        /// <param name="property"> The property to get metadata for. </param>
        /// <returns> The Couchbase-specific metadata for the property. </returns>
        public static ICouchbasePropertyAnnotations Couchbase(this IProperty property)
            => new CouchbasePropertyAnnotations(property);

        /// <summary>
        ///     Gets the Couchbase-specific metadata for a property.
        /// </summary>
        /// <param name="property"> The property to get metadata for. </param>
        /// <returns> The Couchbase-specific metadata for the property. </returns>
        public static CouchbasePropertyAnnotations Couchbase(this IMutableProperty property)
            => (CouchbasePropertyAnnotations)Couchbase((IProperty)property);
    }
}
