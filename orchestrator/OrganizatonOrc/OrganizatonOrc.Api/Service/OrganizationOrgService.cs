using EasyX.Crud.Api.Model;
using EasyX.Data.Api.Entity;
using EasyX.Data.Api.Request;
using Organization.Share.Key;
using OrganizatonOrc.Shared.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrganizatonOrc.Api.Service
{
    public class OrganizationOrgService : IOrchestratorService<OrganizationKey>
    {
        private IModelService<OrganizationKey> organizationService { get; set; }
        private IModelService<EmployeeKey> employeeService { get; set; }
        public OrganizationOrgService(IModelService<OrganizationKey> organizationService, IModelService<EmployeeKey> employeeService)
        {
            this.organizationService = organizationService;
            this.employeeService = employeeService;
        }

        public async Task<dynamic> GetModelAsync(string modelName, IKey<OrganizationKey> keyProvider, CancellationToken cancellationToken = default)
        {
            return await organizationService.GetModelAsync(modelName, keyProvider, cancellationToken)
                                            .ConfigureAwait(false);
        }

        public async Task<dynamic> GetFirstFromModelListAsync(string modelName, IRequest request, CancellationToken cancellationToken = default)
        {
            return await organizationService.GetFirstFromModelListAsync(modelName, request, cancellationToken)
                                            .ConfigureAwait(false);
        }

        public async Task<dynamic> GetModelListAsync(string modelName, IRequest request, CancellationToken cancellationToken = default)
        {
            return await organizationService.GetModelListAsync(modelName, request, cancellationToken)
                                            .ConfigureAwait(false);
        }

        public Task<ITotalModel> GetTotalAsync(string modelName, IRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<TModel> InsertModelAsync<TModel>(TModel model, CancellationToken cancellationToken = default) where TModel : class, IKey<OrganizationKey>
        {
            return await organizationService.InsertModelAsync(model, cancellationToken)
                                            .ConfigureAwait(false);
        }

        public async Task<TModel> UpdateModelAsync<TModel>(TModel model, CancellationToken cancellationToken = default) where TModel : class, IKey<OrganizationKey>
        {
            return await organizationService.UpdateModelAsync(model, cancellationToken)
                                            .ConfigureAwait(false);
        }

        public async Task<IDeleteModel> DeleteAsync(IKey<OrganizationKey> keyProvider, CancellationToken cancellationToken = default)
        {
            return await organizationService.DeleteAsync(keyProvider, cancellationToken)
                                            .ConfigureAwait(false);
        }
    }
}
