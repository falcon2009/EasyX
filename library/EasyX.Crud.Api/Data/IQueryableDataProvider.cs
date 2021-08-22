using EasyX.Data.Api.Entity;
using System;
using System.Linq;

namespace EasyX.Crud.Api.Data
{
    public interface IQueryableDataProvider<TEntity,TKey> : IDataProvider<TKey> where TEntity : class, IKey<TKey> where TKey : class
    {
        public void AddFilter(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryFilter);
    }
}
