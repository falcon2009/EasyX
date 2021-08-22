using System.Threading;
using System.Threading.Tasks;

using EasyX.Data.Api.Entity;


namespace EasyX.Crud.Api.Data
{
    public interface IDataManager<TEntity, TKey> where TEntity : class, IKey<TKey> where TKey : class
    {
        Task<int> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<int> DeleteAsync(IKey<TKey> key, CancellationToken cancellationToken = default);
    }
}
