﻿using System;
using System.Collections.Generic;
using Remotion.Linq.Clauses;

namespace Couchbase.EntityFramework.QueryGeneration
{
    /// <summary>
    /// Provides unique names to use in N1QL queries for each IQuerySource extent
    /// </summary>
    internal class N1QlExtentNameProvider
    {
        private const string ExtentNameFormat = "Extent{0}";

        private int _extentIndex = 0;
        private readonly Dictionary<IQuerySource, string> _extentDictionary = new Dictionary<IQuerySource, string>();

        /// <summary>
        /// If non-null, prefixes all extent names.  I.e. If set to "`p`." then `Extent1` becomes `p`.`Extent1`
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Provides the extent name for a given query source
        /// </summary>
        /// <param name="querySource">IQuerySource for which to get the extent name</param>
        /// <returns>The escaped extent name for the N1QL query</returns>
        public string GetExtentName(IQuerySource querySource)
        {
            if (querySource == null)
            {
                throw new ArgumentNullException("querySource");
            }

            if (Prefix != null)
            {
                return Prefix + GetExtentNameUnprefixed(querySource);
            }
            else
            {
                return GetExtentNameUnprefixed(querySource);
            }
        }

        /// <summary>
        /// Provides the extent name for a given query source, before the Prefix is applied
        /// </summary>
        /// <param name="querySource">IQuerySource for which to get the extent name</param>
        /// <returns>The escaped extent name for the N1QL query</returns>
        private string GetExtentNameUnprefixed(IQuerySource querySource)
        {
            string extentName;
            if (!_extentDictionary.TryGetValue(querySource, out extentName))
            {
                extentName = GetNextExtentName();

                _extentDictionary.Add(querySource, extentName);
            }

            return extentName;
        }

        /// <summary>
        /// Links two extents together so they share the same name
        /// </summary>
        /// <param name="primaryExtent">Extent to link to, which may or may not already have a name</param>
        /// <param name="secondaryExtent">New extent to share the name of the primaryExtent</param>
        /// <returns>
        /// Primarily used when handling join clauses that join to subqueries.  This allows the subquery from
        /// clause to share the same name as the join clause itself, since they are being merged into a single
        /// join clause in the N1QL query output.
        /// </returns>
        public void LinkExtents(IQuerySource primaryExtent, IQuerySource secondaryExtent)
        {
            if (primaryExtent == null)
            {
                throw new ArgumentNullException("primaryExtent");
            }
            if (secondaryExtent == null)
            {
                throw new ArgumentNullException("secondaryExtent");
            }

            if (_extentDictionary.ContainsKey(secondaryExtent))
            {
                throw new InvalidOperationException("The given secondaryExtent has already been generated a unique extent name");
            }

            _extentDictionary.Add(secondaryExtent, GetExtentNameUnprefixed(primaryExtent));
        }

        /// <summary>
        /// Generates a one-time use extent name, which isn't linked to an IQuerySource
        /// </summary>
        /// <returns>The escaped extent name for the N1QL query</returns>
        public string GetUnlinkedExtentName()
        {
            return GetNextExtentName();
        }

        public void SetBlankExtentName(IQuerySource querySource)
        {
            _extentDictionary[querySource] = "";
        }

        public void SetExtentName(IQuerySource querySource, string extentName)
        {
            if (querySource == null)
            {
                throw new ArgumentNullException("extentName");
            }
            if (string.IsNullOrEmpty(extentName))
            {
                return;
            }

            if (_extentDictionary.ContainsKey(querySource))
            {
                throw new InvalidOperationException("Cannot set the extent name on a query source which already has a name.");
            }

            _extentDictionary[querySource] = N1QlHelpers.EscapeIdentifier(extentName);
        }

        /// <summary>
        /// Change the extent name of a query source to a newly generated name, replacing any previously generated name.
        /// </summary>
        /// <param name="querySource">IQuerySource for which to get a new extent name</param>
        /// <returns>The escaped extent name for the N1QL query</returns>
        public string GenerateNewExtentName(IQuerySource querySource)
        {
            if (querySource == null)
            {
                throw new ArgumentNullException("querySource");
            }

            // Remove the extent name, if already generated
            _extentDictionary.Remove(querySource);

            // Generate and return a new extent name
            return GetExtentName(querySource);
        }

        private string GetNextExtentName()
        {
            return N1QlHelpers.EscapeIdentifier(string.Format(ExtentNameFormat, ++_extentIndex));
        }
    }
}
