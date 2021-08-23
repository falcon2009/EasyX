using AutoMapper;
using EasyX.Crud.Api.Data;
using EasyX.ModelService;
using Organization.Share.Key;
using Organization.Share.Model;
using Organization.Storage.EntityModel;

namespace Organization.Storage.Service
{
    public class OrganizationService : ModelServiceEFRepoGeneric<Entity.Organization, OrganizationKey>
    {
        public OrganizationService(IQueryableDataProvider<Entity.Organization, OrganizationKey> dataProvider, IDataManager<Entity.Organization, OrganizationKey> dataManager, IMapper mapper) : base(dataProvider, dataManager, mapper)
        { }

        protected override void ConfigureService()
        {
            BindModel<OrganizationModel, Entity.Organization>();
            BindModel<OrganizationWithEmployeeModel, OrganizationWithEmployee>();
        }
    }
}
