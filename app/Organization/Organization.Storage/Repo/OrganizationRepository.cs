using EasyX.Data.EF;
using Microsoft.EntityFrameworkCore;
using Organization.Share.Key;
using Organization.Storage.EntityModel;
using System;
using System.Linq;

namespace Organization.Storage.Repo
{
    public class OrganizationRepository : EFRepositoryGeneric<ApiDbContext, Entity.Organization, OrganizationKey>
    {
        public OrganizationRepository(ApiDbContext context) : base(context)
        { }

        protected override string DefaultSortField => "CreatedOn";

        protected override void ConfigureRepository()
        {
            static IQueryable<OrganizationLookup> selectOrganizationLookup(IQueryable<Entity.Organization> query) => 
                query.Select(entity => new OrganizationLookup { Id = entity.Id, Titlte = entity.Titlte });

            AddQuery(nameof(Entity.Organization), DbSet);
            AddQuery(nameof(OrganizationWithEmployee), DbSet.Include(entity=>entity.EmployeeList).ThenInclude(entity=>entity.Position));
            AddQueryForModel(selectOrganizationLookup, DbSet);
        }
    }
}
