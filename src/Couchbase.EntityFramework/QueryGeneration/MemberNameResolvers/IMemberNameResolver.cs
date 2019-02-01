using System.Reflection;

namespace Couchbase.EntityFramework.QueryGeneration.MemberNameResolvers
{
    internal interface IMemberNameResolver
    {
        bool TryResolveMemberName(MemberInfo member, out string memberName);
    }
}
