using System.Threading;
using System.Threading.Tasks;

using EasyX.Data.Api.Entity;
using EasyX.Data.Api.Request;

namespace EasyX.Crud.Api.Model
{
    public interface IModelManager<in TKey> where TKey : class
    {
        Task<TModel> InsertModelAsync<TModel>(TModel model, CancellationToken cancellationToken = default) where TModel : class, IKey<TKey>;
        Task<TModel> UpdateModelAsync<TModel>(TModel model, CancellationToken cancellationToken = default) where TModel : class, IKey<TKey>;
        Task<IDeleteModel> DeleteAsync(IKey<TKey> keyProvider, CancellationToken cancellationToken = default);
    }
}
