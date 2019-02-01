using System;
using Couchbase.EntityFramework.QueryGeneration;
using Remotion.Linq.Clauses.StreamedData;

namespace Couchbase.EntityFramework.Operators
{
    /// <summary>
    /// Implementation of <see cref="IStreamedData"/> for a query where <see cref="ToQueryRequestResultOperator"/>
    /// was applied.  Value will be a <see cref="LinqQueryRequest"/>.
    /// </summary>
    internal class LinqQueryRequestValue : IStreamedData
    {
        public IStreamedDataInfo DataInfo { get; private set; }

        public object Value { get; private set; }

        public LinqQueryRequestValue(LinqQueryRequest value, LinqQueryRequestDataInfo linqQueryRequestInfo)
        {
            if (linqQueryRequestInfo == null)
            {
                throw new ArgumentNullException("linqQueryRequestInfo");
            }

            Value = value;
            DataInfo = linqQueryRequestInfo;
        }
    }
}
