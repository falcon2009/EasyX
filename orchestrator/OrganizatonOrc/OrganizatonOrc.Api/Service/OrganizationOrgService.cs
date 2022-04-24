using EasyX.Crud.Api.Model;
using EasyX.Infra.Api;
using EasyX.ModelService;
using Organization.Share.Key;
using Organization.Share.Model;
using System.Collections.Generic;
using System.Linq;
using Person.Share.Key;
using EasyX.Data.Api.Entity;
using System.Threading.Tasks;
using EasyX.Data.Api.Request;
using EasyX.Data.Core.Request;
using EasyX.Infra.Extension;
using EasyX.Data.Api.Enum;
using Person.Share.Model.Person;
using OrganizatonOrc.Shared.Model;
using AutoMapper;
using Person.Share.Model.PersonContact;
using System.Threading;

namespace OrganizatonOrc.Api.Service
{
    public class OrganizationOrgService : OrchestratorServiceGeneric<OrganizationKey>
    {
        private IModelService<OrganizationKey> organizationService { get; set; }

        private IModelService<EmployeeKey> employeeService { get; set; }

        private IModelService<PersonContactKey> personContactService {get; set;}

        private IMapper mapper;

        public OrganizationOrgService(ITypeResolver typeResolver, 
            IModelService<OrganizationKey> organizationService, 
            IModelService<EmployeeKey> employeeService,
            IModelService<PersonContactKey> personContactService,
            IMapper mapper) : base(typeResolver)
        {
            this.organizationService = organizationService;
            this.employeeService = employeeService;
            this.personContactService = personContactService;
            this.mapper = mapper;
        }

        private void aaa()
        {
            List<OrganizationLookupModel> modelList = new List<OrganizationLookupModel>();
            modelList.Where(item => item != null && !string.IsNullOrEmpty(item.Titlte)).ToList();
        }

        #region protected
        protected override void ConfigureService()
        {
            GetModelDataFlow.Add("OrganizationLookupModel", (modelName, key, cancelationToken) => organizationService.GetModelAsync("OrganizationLookupModel", key, cancelationToken));
            GetModelDataFlow.Add("OrganizationModel", (modelName, key, cancelationToken) => organizationService.GetModelAsync("OrganizationModel", key, cancelationToken));
            GetModelDataFlow.Add("OrganizationWithEmployeeOrcModel", (modelName, key, cancelationToken) => GetOrganizationWithEmployeeOrcModel(string.Empty, key, cancelationToken));

            GetModelListDataFlow.Add("OrganizationLookupModel", (modelName, request, cancelationToken) => organizationService.GetModelListAsync("OrganizationLookupModel", request, cancelationToken));
            GetModelListDataFlow.Add("OrganizationModel", (modelName, request, cancelationToken) => organizationService.GetModelListAsync("OrganizationModel", request, cancelationToken));
            #endregion
        }

        #region private
        private async Task<dynamic> GetOrganizationWithEmployeeOrcModel(string modelName, IKey<OrganizationKey> keyProvider, CancellationToken token)
        {
            OrganizationWithEmployeeModel organizationModel = await organizationService.GetModelAsync("OrganizationWithEmployeeModel", keyProvider, token)   
                                                                                       .ConfigureAwait(false);
            if (organizationModel == default)
            {
                return default;
            }

            OrganizationWithEmployeeOrcModel resultModel = mapper.Map<OrganizationWithEmployeeOrcModel>(organizationModel);
            List<PersonContactModel> personContactList = null;
            if (resultModel.EmployeeList.Any())
            {
                IRequest request = new Request()
                {
                    FilterField = new List<string>() { "PersonId" },
                    FilterValue = new List<string>() { resultModel.EmployeeList.Select(item => item.PersonId).ToList().ToArrayParam() },
                    FilterType = new List<FilterType>() { FilterType.Equal }
                };
                personContactList = await personContactService.GetModelListAsync("PersonContactModel", request)
                                                              .ConfigureAwait(false);
            }

            return resultModel;
        }
        #endregion
    }
}
