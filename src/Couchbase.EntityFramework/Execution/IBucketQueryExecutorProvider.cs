namespace Couchbase.EntityFramework.Execution
{
    /// <summary>
    /// Provides access to an <see cref="IBucketQueryExecutor"/>.
    /// </summary>
    internal interface IBucketQueryExecutorProvider
    {
        /// <summary>
        /// Get the <see cref="IBucketQueryExecutor"/>.
        /// </summary>
        IBucketQueryExecutor BucketQueryExecutor { get; }
    }
}
