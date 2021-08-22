using System;

namespace EasyX.Infra.Api
{
    public interface ITypeResolver
    {
        Type GetType(string typeName);
    }

    public interface ITypeResolver<T>
    {
        Type GetType(string typeName);
    }
}
