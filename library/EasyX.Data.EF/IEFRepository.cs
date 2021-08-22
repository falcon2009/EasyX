using EasyX.Crud.Api.Data;
using EasyX.Data.Api.Entity;
using Microsoft.EntityFrameworkCore;

namespace EasyX.Data.EF
{
    public interface IEFRepository<TEntity, TKey> : IQueryableDataProvider<TEntity, TKey>, IDataManager<TEntity, TKey> where TEntity : class, IKey<TKey> where TKey : class
    {
        DbContext GetContext();
    }
}
