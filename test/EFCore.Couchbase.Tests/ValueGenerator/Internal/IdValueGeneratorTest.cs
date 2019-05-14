// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Couchbase.TestUtilities;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Couchbase.ValueGenerator.Internal
{
    public class IdValueGeneratorTest
    {
        [Fact]
        public void Generated_ids_do_not_clash()
        {
            var modelBuilder = CouchbaseTestHelpers.Instance.CreateConventionBuilder();

            modelBuilder.Entity<Blog>().HasKey(p => new { p.OtherId, p.Id });
            modelBuilder.Entity<Post>().HasKey(p => new { p.OtherId, p.Id });

            var model = modelBuilder.FinalizeModel();

            var blogIdProperty = model.FindEntityType(typeof(Blog)).FindProperty("id");
            var postIdProperty = model.FindEntityType(typeof(Post)).FindProperty("id");

            var ids = new HashSet<string>();
            ids.Add((string)CouchbaseTestHelpers.Instance.CreateInternalEntry(model, EntityState.Added, new Blog { Id = 1, OtherId = 1 })
                [blogIdProperty]);
            ids.Add((string)CouchbaseTestHelpers.Instance.CreateInternalEntry(model, EntityState.Added, new Blog { Id = 1, OtherId = 1 })
                [blogIdProperty]);
            ids.Add((string)CouchbaseTestHelpers.Instance.CreateInternalEntry(model, EntityState.Added, new Post { Id = "1", OtherId = "1" })
                [postIdProperty]);
            ids.Add((string)CouchbaseTestHelpers.Instance.CreateInternalEntry(model, EntityState.Added, new Post { Id = "1", OtherId = "1|" })
                [postIdProperty]);
            ids.Add((string)CouchbaseTestHelpers.Instance.CreateInternalEntry(model, EntityState.Added, new Post { Id = "|1", OtherId = "1" })
                [postIdProperty]);

            Assert.Equal(4, ids.Count);
        }

        private class Blog
        {
            public int Id { get; set; }
            public int OtherId { get; set; }
        }

        private class Post
        {
            public string Id { get; set; }
            public string OtherId { get; set; }
        }
    }
}
