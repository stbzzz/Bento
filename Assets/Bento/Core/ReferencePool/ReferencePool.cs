using System;
using System.Collections.Generic;

namespace Bento
{
    public static partial class ReferencePool
    {
        private static readonly Dictionary<Type, ReferenceQueue> m_ReferenceQueues = new();

        public static int Count => m_ReferenceQueues.Count;

        public static void ClearAll()
        {
            lock (m_ReferenceQueues)
            {
                foreach (var referenceQueue in m_ReferenceQueues)
                {
                    referenceQueue.Value.RemoveAll();
                }
                m_ReferenceQueues.Clear();
            }
        }

        public static T Get<T>() where T : class, IReference, new()
        {
            return GetReferenceQueue(typeof(T)).Get<T>();
        }

        public static IReference Get(Type referenceType)
        {
            return GetReferenceQueue(referenceType).Get();
        }

        public static void Put(IReference reference)
        {
            if (reference == null)
            {
                throw new BentoException("ReferenceType is invalid.");
            }
            Type referenceType = reference.GetType();
            GetReferenceQueue(referenceType).Put(reference);
        }

        public static void Add<T>(int count) where T : class, IReference, new()
        {
            GetReferenceQueue(typeof(T)).Add<T>(count);
        }

        public static void Add(Type referenceType, int count)
        {
            GetReferenceQueue(referenceType).Add(count);
        }

        public static void Remove<T>(int count) where T : class, IReference
        {
            GetReferenceQueue(typeof(T)).Remove(count);
        }

        public static void Remove(Type referenceType, int count)
        {
            GetReferenceQueue(referenceType).Remove(count);
        }

        public static void RemoveAll<T>() where T : class, IReference
        {
            GetReferenceQueue(typeof(T)).RemoveAll();
        }

        public static void RemoveAll(Type referenceType)
        {
            GetReferenceQueue(referenceType).RemoveAll();
        }

        private static ReferenceQueue GetReferenceQueue(Type referenceType)
        {
            if (referenceType == null)
            {
                throw new BentoException("ReferenceType is invalid.");
            }
            ReferenceQueue referenceQueue = null;
            lock (m_ReferenceQueues)
            {
                if (!m_ReferenceQueues.TryGetValue(referenceType, out referenceQueue))
                {
                    referenceQueue = new ReferenceQueue(referenceType);
                    m_ReferenceQueues.Add(referenceType, referenceQueue);
                }
            }
            return referenceQueue;
        }
    }
}