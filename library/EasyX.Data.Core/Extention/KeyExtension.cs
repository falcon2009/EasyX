using EasyX.Data.Api.Entity;
using EasyX.Infra;
using System;
using System.Reflection;
using System.Text;

namespace EasyX.Data.Core.Extention
{
    public static class KeyExtension
    {
        private const string routePrefix = "/";
        private const string queryPrefix = "&";
        public static string ToRoute<TKey>(this IKey<TKey> keyProvider) where TKey : class
        {
            if (keyProvider == null)
            {
                throw new ArgumentNullException(nameof(keyProvider), Constant.Errors.KeyNull);
            }

            StringBuilder builder = new();
            foreach (PropertyInfo propertyInfo in keyProvider.Key.GetType().GetProperties())
            {
                if (builder.Length != 0)
                {
                    builder.Append(routePrefix);
                }
                builder.Append(propertyInfo.GetValue(keyProvider.Key));
            }

            return builder.ToString();
        }

        public static string ToQuery<TKey>(this IKey<TKey> keyProvider) where TKey : class
        {
            if (keyProvider == null)
            {
                throw new ArgumentNullException(nameof(keyProvider), Constant.Errors.KeyNull);
            }

            StringBuilder builder = new();
            foreach (PropertyInfo propertyInfo in keyProvider.Key.GetType().GetProperties())
            {
                if (builder.Length != 0)
                {
                    builder.Append(queryPrefix);
                }
                builder.Append($"{propertyInfo.Name}={propertyInfo.GetValue(keyProvider.Key)}");
            }

            return builder.ToString();
        }
    }
}
