using System.Threading;
using System.Threading.Tasks;

using EasyX.Data.Api.Entity;
using EasyX.Data.Api.Request;

namespace EasyX.Crud.Api.Model
{
    public interface IModelProvider<in TKey> where TKey : class
    {
        Task<dynamic> GetModelAsync(string modelName, IKey<TKey> keyProvider, CancellationToken cancellationToken = default);
        Task<dynamic> GetModelListAsync(string modelName, IRequest request, CancellationToken cancellationToken = default);
        Task<dynamic> GetFirstFromModelListAsync(string modelName, IRequest request, CancellationToken cancellationToken = default);
        Task<ITotalModel> GetTotalAsync(string modelName, IRequest request, CancellationToken cancellationToken = default);
    }
}
