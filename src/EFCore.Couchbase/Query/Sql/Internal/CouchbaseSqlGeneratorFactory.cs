// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Couchbase.Query.Expressions.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Couchbase.Query.Sql.Internal
{
    public class CouchbaseSqlGeneratorFactory : ISqlGeneratorFactory
    {
        public CouchbaseSqlGeneratorFactory(ITypeMappingSource typeMappingSource)
        {
            TypeMappingSource = typeMappingSource;
        }

        protected virtual ITypeMappingSource TypeMappingSource { get; }

        public ISqlGenerator CreateDefault(SelectExpression selectExpression)
        {
            return new CouchbaseSqlGenerator(selectExpression, TypeMappingSource);
        }

        public ISqlGenerator CreateFromSql(SelectExpression selectExpression, string sql, Expression arguments)
        {
            throw new System.NotImplementedException();
        }
    }
}
