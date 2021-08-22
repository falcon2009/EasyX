﻿using AutoMapper;
using EasyX.Crud.Api.Data;
using EasyX.ModelService;
using Person.Share.Key;
using Person.Share.Model.Person;
using Person.Storage.EntityModel;

namespace Person.Storage.Model
{
    public class PersonModelService : ModelServiceEFRepoGeneric<Entity.Person, PersonKey>
    {
        public PersonModelService(IQueryableDataProvider<Entity.Person, PersonKey> dataProvider, IDataManager<Entity.Person, PersonKey> dataManager, IMapper mapper):base(dataProvider, dataManager, mapper)
        { }

        protected override void ConfigureService()
        {
            BindModel<PersonModel, Entity.Person>();
            BindModel<PersonLookupModel, PersonLookup>();
        }
    }
}
