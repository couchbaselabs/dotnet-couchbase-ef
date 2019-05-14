﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Couchbase.Metadata
{
    public interface ICouchbaseEntityTypeAnnotations
    {
        string ContainerName { get; }
        IProperty DiscriminatorProperty { get; }
        object DiscriminatorValue { get; }
        string ContainingPropertyName { get; }
    }
}
