using AutoMapper;
using EasyX.Data.Core.Tool;
using Person.Share.Key;
using Person.Share.Model.Person;
using Person.Share.Model.PersonContact;
using Entity = Person.Storage.Entity;

namespace Person.Storage.Mapping
{
    public class PersonMapping : Profile
    {
        public PersonMapping()
        {
            CreateMap<Entity.Person, PersonModel>();
            CreateMap<PersonModel, Entity.Person>()
                .ForMember(entity => entity.PersonContactList, option => option.Ignore())
                .AfterMap<MapPersonFromPersonModel>();

            CreateMap<Entity.PersonContact, PersonContactModel>();
        }

        private class MapPersonFromPersonModel : IMappingAction<PersonModel, Entity.Person>
        {
            public void Process(PersonModel source, Entity.Person destination, ResolutionContext context)
            {
                MappingHelper.JoinOneToMany<Entity.Person, PersonModel, Entity.PersonContact, PersonContactModel, PersonContactKey>
                    (destination, source, entity => entity.PersonContactList, model => model.PersonContactList, context.Mapper, false, false);
            }
        }
    }
}
