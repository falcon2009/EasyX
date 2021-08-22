using EasyX.Data.Api.Entity;

namespace EasyX.Crud.Api.Data
{
    public interface IRepository<TEntity, TKey> : IDataProvider<TKey>, IDataManager<TEntity, TKey> 
        where TEntity : class, IKey<TKey> where TKey : class
    {
    }
}
