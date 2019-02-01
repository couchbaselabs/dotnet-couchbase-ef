
namespace Couchbase.EntityFramework.Proxies
{
    internal interface ITrackedDocumentNodeCallback
    {
        void DocumentModified(ITrackedDocumentNode mutatedDocument);
    }
}
