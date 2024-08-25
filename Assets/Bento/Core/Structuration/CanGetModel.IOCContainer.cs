using System;
using System.Collections.Generic;

namespace Bento
{
    public abstract partial class CanGetModel
    {
        private sealed class IOCContainer
        {
            private readonly Dictionary<Type, object> m_Instances = new();

            public void Set<T>(T instance)
            {
                var key = typeof(T);
                if (m_Instances.ContainsKey(key))
                {
                    m_Instances[key] = instance;
                }
                else
                {
                    m_Instances.Add(key, instance);
                }
            }

            public T Get<T>() where T : class
            {
                var key = typeof(T);
                if (m_Instances.TryGetValue(key, out var instance))
                {
                    return instance as T;
                }
                return null;
            }
        }
    }
}