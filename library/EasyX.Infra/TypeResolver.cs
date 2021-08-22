using EasyX.Infra.Api;
using System;
using System.Collections.Concurrent;

namespace EasyX.Infra
{
    public class TypeResolver : ITypeResolver
    {
        private readonly ConcurrentDictionary<string, Type> typeStorage = new ConcurrentDictionary<string, Type>();
        public TypeResolver()
        { }
        public Type GetType(string typeName)
        {
            bool hasType = typeStorage.ContainsKey(typeName);
            if (!hasType)
            {
                return null;
            }

            return typeStorage[typeName];
        }
        public void RegisterContract<T>() where T:class
        {
            Type type = typeof(T);
            string typeName = type.Name;
            if (!typeStorage.ContainsKey(typeName))
            {
                typeStorage.TryAdd(typeName, type);
            }
        }
    }
}
