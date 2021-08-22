using AutoMapper;
using EasyX.Crud.Api.Data;
using EasyX.Data.Api.Request;
using EasyX.ModelService;
using System.Linq;
using UAC.Share.Key;
using UAC.Share.Model;
using UAC.Storage.Entity;
using UAC.Storage.EntityModel;

namespace UAC.Storage.Service
{
    public class RoleService : ModelServiceEFRepoGeneric<Role, RoleKey>
    {
        public RoleService(IQueryableDataProvider<Role, RoleKey> dataProvider, IDataManager<Role, RoleKey> dataManager, IMapper mapper)
            : base(dataProvider, dataManager, mapper)
        { }

        protected override void ConfigureService()
        {
            BindModel<RoleModel, Role>();
            BindModel<RoleLookupModel, RoleLookup>();
        }
    }
}
