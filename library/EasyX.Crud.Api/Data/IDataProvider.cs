using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using EasyX.Data.Api.Entity;
using EasyX.Data.Api.Request;

namespace EasyX.Crud.Api.Data
{
    public interface IDataProvider<TKey> where TKey : class
    {
        Task<TModel> GetModelAsync<TModel>(IKey<TKey> key, bool isTrackable = false, CancellationToken cancellationToken = default) where TModel : class, IKey<TKey>;
        Task<TModel> GetFirstFromModelAsync<TModel>(IRequest request, CancellationToken cancellationToken = default) where TModel : class;
        Task<List<TModel>> GetModelListAsync<TModel>(IRequest request, CancellationToken cancellationToken = default) where TModel : class;
        Task<int> GetTotalAsync<TModel>(IRequest request, CancellationToken cancellationToken = default) where TModel : class;
    }
}
