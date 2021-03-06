﻿using System;
using Couchbase.EntityFramework.Filters;

namespace Couchbase.EntityFramework
{
    /// <summary>
    /// Options to control queries against an <see cref="IBucketContext"/>.
    /// </summary>
    [Flags]
    public enum BucketQueryOptions
    {
        /// <summary>
        /// No special options, use default behavior.
        /// </summary>
        None = 0,

        /// <summary>
        /// Supress all registered filters in the <see cref="DocumentFilterManager"/>.
        /// </summary>
        SuppressFilters = 1
    }
}
