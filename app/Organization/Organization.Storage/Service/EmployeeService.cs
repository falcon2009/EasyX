using AutoMapper;
using EasyX.Crud.Api.Data;
using EasyX.ModelService;
using Organization.Share.Key;
using Organization.Share.Model;

namespace Organization.Storage.Service
{
    public class EmployeeService : ModelServiceEFRepoGeneric<Entity.Employee, EmployeeKey>
    {
        public EmployeeService(IQueryableDataProvider<Entity.Employee, EmployeeKey> dataProvider, IDataManager<Entity.Employee, EmployeeKey> dataManager, IMapper mapper) : base(dataProvider, dataManager, mapper)
        { }

        protected override void ConfigureService()
        {
            BindModel<EmployeeModel, Entity.Employee>();
        }
    }
}
