using EasyX.Infra.Api;
using System;
using System.Collections.Concurrent;

namespace EasyX.Infra.Core
{
    public class TypeResolver : ITypeResolver
    {
        private readonly ConcurrentDictionary<string, Type> typeStorage = new ConcurrentDictionary<string, Type>();

        public Type GetType(string typeName)
        {
            return typeStorage.ContainsKey(typeName) ? typeStorage[typeName] : null;
        }
        public void RegisterContract<T>() where T:class
        {
            RegisterContract(typeof(T));

        }
        public void RegisterContract(Type type)
        {
            if (!typeStorage.ContainsKey(type.Name))
            {
                typeStorage.TryAdd(type.Name, type);
            }
        }
    }
}
