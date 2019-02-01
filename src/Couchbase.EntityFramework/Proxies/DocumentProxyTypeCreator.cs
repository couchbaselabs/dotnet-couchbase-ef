﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Couchbase.Core.Serialization;

namespace Couchbase.EntityFramework.Proxies
{
    /// <summary>
    /// Implements an <see cref="ICustomObjectCreator"/> which creates proxies that implement <see cref="ITrackedDocumentNode"/>.
    /// </summary>
    internal class DocumentProxyTypeCreator : ICustomObjectCreator
    {
        private static readonly Assembly Mscorlib = typeof(string).GetTypeInfo().Assembly;
        private static readonly Assembly Couchbase = typeof(Couchbase.Core.IBucket).GetTypeInfo().Assembly;

        public bool CanCreateObject(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            Type collectionType;
            if (IsProxyableCollection(type, out collectionType))
            {
                // Proxy collection interfaces
                return true;
            }

            if (!type.GetTypeInfo().IsClass || type.GetTypeInfo().IsSealed || typeof(Delegate).IsAssignableFrom(type))
            {
                return false;
            }

            var interfaces = type.GetInterfaces();
            if (interfaces.Any(p => p.GetTypeInfo().UnderlyingSystemType == typeof (IBucketContext)))
            {
                //don't proxy the context
                return false;
            }

            // Don't proxy classes from mscorlib or Couchbase SDK, but proxy everything else
            return !type.GetTypeInfo().Assembly.Equals(Mscorlib) && !type.GetTypeInfo().Assembly.Equals(Couchbase);
        }

        public object CreateObject(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            // For performance, don't repeat the CanCreateObject tests
            // We must assume that CanCreateObject was verified before this method was called

            object document;
            Type collectionType;

            if (IsProxyableCollection(type, out collectionType))
            {
                document = Activator.CreateInstance(collectionType);
            }
            else
            {
                document = DocumentProxyManager.Default.CreateProxy(type);
            }

            ((ITrackedDocumentNode)document).IsDeserializing = true;

            return document;
        }

        #region Collection Helpers

        /// <summary>
        /// Caches collection types for reuse to prevent excess reflection.
        /// Key is the document type, value is the resulting collection type.
        /// Null value indicates that the document type is not a proxyable collection.
        /// </summary>
        private readonly Dictionary<Type, Type> _collectionTypeCache = new Dictionary<Type, Type>();

        private bool IsProxyableCollection(Type documentType, out Type collectionType)
        {
            if (_collectionTypeCache.TryGetValue(documentType, out collectionType))
            {
                return collectionType != null;
            }

            Type elementType = null;

            // Check to see if the documentType is ICollection<T> or IList<T>, and extract the element type

            if (documentType.GetTypeInfo().IsInterface && documentType.GetTypeInfo().IsGenericType)
            {
                Type genericDefinition = documentType.GetGenericTypeDefinition();

                if ((genericDefinition == typeof(ICollection<>)) || (genericDefinition == typeof(IList<>)))
                {
                    elementType = documentType.GenericTypeArguments[0];
                }
            }

            if (elementType != null)
            {
                // This is a valid collection type, so build the resulting collectionType
                // And store in the cache for reuse

                collectionType = typeof (DocumentCollection<>).MakeGenericType(elementType);

                _collectionTypeCache.Add(documentType, collectionType);

                return true;
            }
            else
            {
                // This is not a valid collection type.  Store this fact in the cache for reuse.

                _collectionTypeCache.Add(documentType, null);

                return false;
            }
        }

        #endregion

    }
}
