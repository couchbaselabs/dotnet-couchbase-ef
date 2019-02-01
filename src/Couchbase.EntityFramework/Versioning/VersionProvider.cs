namespace Couchbase.EntityFramework.Versioning
{
    /// <summary>
    /// Singleton for the <see cref="IVersionProvider"/> implementation in use for query generation.
    /// </summary>
    internal static class VersionProvider
    {
        /// <summary>
        /// Singleton for the <see cref="IVersionProvider"/> implementation in use for query generation.
        /// </summary>
        public static IVersionProvider Current { get; set; }

        static VersionProvider()
        {
            Current = new DefaultVersionProvider();
        }
    }
}
