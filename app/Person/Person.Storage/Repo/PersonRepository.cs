using EasyX.Data.EF;
using Microsoft.EntityFrameworkCore;
using Person.Share.Key;
using Person.Storage.EntityModel;
using System.Linq;
using Entity = Person.Storage.Entity;

namespace Person.Storage.Repo
{
    public class PersonRepository : EFRepositoryGeneric<ApiDbContext, Entity.Person, PersonKey>
    {
        public PersonRepository(ApiDbContext context) : base(context)
        { }
        protected override string DefaultSortField => "CreatedOn";

        protected override void ConfigureRepository()
        {
            static IQueryable<PersonLookup> selectPersonLookup(IQueryable<Entity.Person> query) =>
                query.Select(entity => new PersonLookup()
                {
                    Id = entity.Id,
                    FullName = $"{entity.FirstName} {entity.LastName}"
                });

            AddQuery(nameof(Entity.Person), DbSet.Include(entity => entity.PersonContactList));
            AddQueryForModel(selectPersonLookup, DbSet);
        }
    }
}
