using System;

namespace Couchbase.EntityFramework
{
    /// <summary>
    /// Implements static helper methods for N1QL queries
    /// </summary>
    public static partial class N1QlFunctions
    {
        /// <summary>
        /// Shortcut for creating an error for methods that are only supported in N1QL, not in .Net.
        /// </summary>
        private static Exception NotSupportedError()
        {
            return new NotSupportedException("This method may only be used in lambda expressions for generating N1QL queries.");
        }
    }
}
