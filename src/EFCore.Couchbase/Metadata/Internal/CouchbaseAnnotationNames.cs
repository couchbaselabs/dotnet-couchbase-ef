// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.EntityFrameworkCore.Couchbase.Metadata.Internal
{
    public static class CouchbaseAnnotationNames
    {
        public const string Prefix = "Couchbase:";
        public const string ContainerName = Prefix + "ContainerName";
        public const string PropertyName = Prefix + "PropertyName";
        public const string DiscriminatorProperty = Prefix + "DiscriminatorProperty";
        public const string DiscriminatorValue = Prefix + "DiscriminatorValue";
    }
}
