using Couchbase.EntityFramework.Extensions;

namespace Couchbase.EntityFramework
{
    /// <summary>
    /// Types of indices supported by N1QL query <see cref="QueryExtensions.UseIndex{T}(System.Linq.IQueryable{T},string,Couchbase.EntityFramework.N1QlIndexType)"/>.
    /// </summary>
    public enum N1QlIndexType
    {
        /// <summary>
        /// Global secondary index
        /// </summary>
        Gsi,

        /// <summary>
        /// View index
        /// </summary>
        View
    }
}
