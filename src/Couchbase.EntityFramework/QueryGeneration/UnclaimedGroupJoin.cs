using Remotion.Linq.Clauses;

namespace Couchbase.EntityFramework.QueryGeneration
{
    internal class UnclaimedGroupJoin
    {

        public JoinClause JoinClause { get; set; }
        public GroupJoinClause GroupJoinClause { get; set; }

    }
}
