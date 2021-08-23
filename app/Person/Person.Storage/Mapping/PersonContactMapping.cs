using AutoMapper;
using Person.Share.Model.PersonContact;

namespace Person.Storage.Mapping
{
    public class PersonContactMapping : Profile
    {
        public PersonContactMapping()
        {
            CreateMap<Entity.PersonContact, PersonContactModel>();
            CreateMap<PersonContactModel, Entity.PersonContact>();
        }
    }
}
