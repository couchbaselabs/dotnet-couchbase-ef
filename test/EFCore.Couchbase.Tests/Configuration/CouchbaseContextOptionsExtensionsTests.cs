using Microsoft.EntityFrameworkCore.Couchbase.Infrastructure.Internal;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Couchbase.Configuration
{
    public class CouchbaseContextOptionsExtensionsTests
    {
//        [Fact]
//        public void Can_create_options_with_specified_region()
//        {
//            var regionName = CouchbaseRegions.EastAsia;
//            var options = new DbContextOptionsBuilder().UseCouchbase(
//                "serviceEndPoint",
//                "authKeyOrResourceToken",
//                "databaseName",
//                o => { o.Region(regionName); });
//
//            var extension = options
//                .Options.FindExtension<CouchbaseOptionsExtension>();
//
//            Assert.Equal(regionName, extension.Region);
//        }

        /// <summary>
        /// The region will be checked by the Couchbase sdk, because the region list is not constant
        /// </summary>
        [Fact]
        public void Can_create_options_with_wrong_region()
        {
            var regionName = "FakeRegion";
            var options = new DbContextOptionsBuilder().UseCouchbase(
                "serviceEndPoint",
                "authKeyOrResourceToken",
                "databaseName",
                o => { o.Region(regionName); });

            var extension = options
                .Options.FindExtension<CouchbaseOptionsExtension>();

            Assert.Equal(regionName, extension.Region);
        }
    }
}
