// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Couchbase.Storage.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Couchbase.TestUtilities
{
    public class CouchbaseTestHelpers : TestHelpers
    {
        protected CouchbaseTestHelpers()
        {
        }

        public static CouchbaseTestHelpers Instance { get; } = new CouchbaseTestHelpers();

        public override IServiceCollection AddProviderServices(IServiceCollection services)
        {
            return services.AddEntityFrameworkCouchbase();
        }

        public override IModelValidator CreateModelValidator()
        {
            var typeMappingSource = new CouchbaseTypeMappingSource(
                TestServiceFactory.Instance.Create<TypeMappingSourceDependencies>());

            return new ModelValidator(
                new ModelValidatorDependencies(
                    typeMappingSource,
                    new MemberClassifier(
                        typeMappingSource,
                        TestServiceFactory.Instance.Create<IParameterBindingFactories>())));
        }

        protected override void UseProviderOptions(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCouchbase(
                TestEnvironment.DefaultConnection,
                TestEnvironment.AuthToken,
                "UnitTests");
        }
    }
}
