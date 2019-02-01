﻿namespace Couchbase.EntityFramework.Utils
{
    internal class ExceptionMsgs
    {
        public const string DocumentNotFound = "Document not found for Id: ";

        public const string KeyAttributeMissing = "No KeyAttribute could be found for the document. Please " +
                                                  "mark a key property with a KeyAttribute";

        public const string KeyNull = "KeyAttribute marks a property which returned null.  Please " +
                                      "be sure the key property is non-null.";

        public const string QueryExecutionException = "An error occurred executing the N1QL query.  See the " +
                                                      "inner exception for details.";

        public const string QueryExecutionUnknownError = "An unknown error occurred executing the N1QL query.";

        public const string QueryExecutionMultipleErrors = "Multiple errors occured executing the N1QL query. " +
                                                           "See the Errors property for details.";
    }
}
#region [ License information          ]

/* ************************************************************
 *
 *    @author Couchbase <info@couchbase.com>
 *    @copyright 2015 Couchbase, Inc.
 *
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *
 *        http://www.apache.org/licenses/LICENSE-2.0
 *
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 *
 * ************************************************************/

#endregion
