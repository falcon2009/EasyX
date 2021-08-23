using AutoMapper;
using EasyX.Crud.Api.Data;
using EasyX.ModelService;
using Organization.Share.Key;
using Organization.Share.Model;

namespace Organization.Storage.Service
{
    public class PositionService : ModelServiceEFRepoGeneric<Entity.Position, PositionKey>
    {
        public PositionService(IQueryableDataProvider<Entity.Position, PositionKey> dataProvider, IDataManager<Entity.Position, PositionKey> dataManager, IMapper mapper) : base(dataProvider, dataManager, mapper)
        { }

        protected override void ConfigureService()
        {
            BindModel<PositionModel, Entity.Position>();
        }
    }
}
