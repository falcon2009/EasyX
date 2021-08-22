using EasyX.Data.EF;
using Person.Share.Key;
using Person.Storage.Entity;
using System;

namespace Person.Storage.Repo
{
    public class PersonContactRepository : EFRepositoryGeneric<ApiDbContext, PersonContact, PersonContactKey>
    {
        public PersonContactRepository(ApiDbContext context) : base(context)
        { }

        protected override string DefaultSortField => "Type";

        protected override void ConfigureRepository()
        {
            AddQuery(nameof(PersonContact), DbSet);
        }
    }
}
