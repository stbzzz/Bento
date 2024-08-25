using System;
using System.Collections.Generic;

namespace Bento
{
    public static partial class ReferencePool
    {
        private sealed class ReferenceQueue
        {
            private readonly Type m_ReferenceType;
            private readonly Queue<IReference> m_References;

            public ReferenceQueue(Type referenceType)
            {
                m_References = new Queue<IReference>();
                m_ReferenceType = referenceType;
            }

            public Type ReferenceType => m_ReferenceType;

            public int UnusedReferenceCount => m_References.Count;

            public int UsingReferenceCount { get; private set; } = 0;

            public int AcquiredReferenceCount { get; private set; } = 0;

            public int ReleasedReferenceCount { get; private set; } = 0;

            public int AddReferenceCount { get; private set; } = 0;

            public int RemoveReferenceCount { get; private set; } = 0;

            public T Get<T>() where T : class, IReference, new()
            {
                if (typeof(T) != m_ReferenceType)
                {
                    throw new BentoException("Type is invalid.");
                }
                UsingReferenceCount++;
                AcquiredReferenceCount++;
                lock (m_References)
                {
                    if (m_References.Count > 0)
                    {
                        return (T)m_References.Dequeue();
                    }
                }
                AddReferenceCount++;
                return new T();
            }

            public IReference Get()
            {

                UsingReferenceCount++;
                AcquiredReferenceCount++;
                lock (m_References)
                {
                    if (m_References.Count > 0)
                    {
                        return m_References.Dequeue();
                    }
                }
                AddReferenceCount++;
                return (IReference)Activator.CreateInstance(m_ReferenceType);
            }

            public void Put(IReference reference)
            {
                reference.Clear();
                lock (m_References)
                {
                    m_References.Enqueue(reference);
                }
                UsingReferenceCount--;
                ReleasedReferenceCount++;
            }

            public void Add<T>(int count) where T : class, IReference, new()
            {
                if (typeof(T) != m_ReferenceType)
                {
                    throw new BentoException("Type is invalid.");
                }

                lock (m_References)
                {
                    AddReferenceCount += count;
                    while (count-- > 0)
                    {
                        m_References.Enqueue(new T());
                    }
                }
            }

            public void Add(int count)
            {
                lock (m_References)
                {
                    AddReferenceCount += count;
                    while (count-- > 0)
                    {
                        m_References.Enqueue((IReference)Activator.CreateInstance(m_ReferenceType));
                    }
                }
            }

            public void Remove(int count)
            {
                lock (m_References)
                {
                    if (count > m_References.Count)
                    {
                        count = m_References.Count;
                    }

                    RemoveReferenceCount += count;
                    while (count-- > 0)
                    {
                        m_References.Dequeue();
                    }
                }
            }

            public void RemoveAll()
            {
                lock (m_References)
                {
                    RemoveReferenceCount += m_References.Count;
                    m_References.Clear();
                }
            }
        }
    }
}