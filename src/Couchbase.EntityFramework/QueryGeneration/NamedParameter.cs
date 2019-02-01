namespace Couchbase.EntityFramework.QueryGeneration
{
    internal sealed class NamedParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
