using AutoMapper;
using EasyX.Crud.Api.Data;
using EasyX.ModelService;
using Person.Share.Key;
using Person.Share.Model.PersonContact;

namespace Person.Storage.Service
{
    public class PersonContactService : ModelServiceEFRepoGeneric<Entity.PersonContact, PersonContactKey>
    {
        public PersonContactService(IQueryableDataProvider<Entity.PersonContact, PersonContactKey> dataProvider, IDataManager<Entity.PersonContact, PersonContactKey> dataManager, IMapper mapper) : base(dataProvider, dataManager, mapper)
        { }
        protected override void ConfigureService()
        {
            BindModel<PersonContactModel, Entity.PersonContact>();
        }
    }
}
