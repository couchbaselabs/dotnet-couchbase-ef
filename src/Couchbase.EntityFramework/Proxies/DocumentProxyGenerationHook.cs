﻿using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace Couchbase.EntityFramework.Proxies
{
    /// <summary>
    /// Hook to control generation of proxies for document objects
    /// </summary>
    class DocumentProxyGenerationHook : IProxyGenerationHook
    {
        public override bool Equals(object obj)
        {
            // Always treat DocumentProxyGenerationHook objects as equal
            // So that the generated proxy type is cached and reused

            return obj is DocumentProxyGenerationHook;
        }

        public override int GetHashCode()
        {
            // Always treat DocumentProxyGenerationHook objects as equal
            // So that the generated proxy type is cached and reused

            return 0;
        }

        public void MethodsInspected()
        {
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
            // TODO Logging of non-virtual property setters for debugging purposes
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            if (type == typeof (ITrackedDocumentNode))
            {
                // All calls for ITrackedDocumentNode should go to the interceptor

                return true;
            }

            // Only proxy getters and setters for properties on the document
            // We must proxy getters or serializing the document back to Couchbase has problems

            return methodInfo.IsSpecialName && (methodInfo.Name.StartsWith("set_") || methodInfo.Name.StartsWith("get_"));
        }
    }
}
