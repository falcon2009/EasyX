using EasyX.Crud.Api.Model;
using EasyX.Infra.Api;
using EasyX.ModelService;
using Organization.Share.Key;

namespace OrganizatonOrc.Api.Service
{
    public class OrganizationOrgService : OrchestratorServiceGeneric<OrganizationKey>
    {
        private IModelService<OrganizationKey> organizationService { get; set; }
        private IModelService<EmployeeKey> employeeService { get; set; }
        public OrganizationOrgService(ITypeResolver typeResolver, IModelService<OrganizationKey> organizationService, IModelService<EmployeeKey> employeeService) : base(typeResolver)
        {
            this.organizationService = organizationService;
            this.employeeService = employeeService;
        }



        #region protected
        protected override void ConfigureService()
        {
            GetModelDataFlow.Add("OrganizationLookupModel", (modelName, key, cancelationToken) => organizationService.GetModelAsync(modelName, key, cancelationToken));
            GetModelDataFlow.Add("OrganizationModel", (modelName, key, cancelationToken) => organizationService.GetModelAsync(modelName, key, cancelationToken));

            GetModelListDataFlow.Add("OrganizationLookupModel", (modelName, request, cancelationToken) => organizationService.GetModelListAsync(modelName, request, cancelationToken));
            GetModelListDataFlow.Add("OrganizationModel", (modelName, request, cancelationToken) => organizationService.GetModelListAsync(modelName, request, cancelationToken));
            #endregion
        }
    }
}
