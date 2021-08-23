using EasyX.Data.EF;
using Microsoft.EntityFrameworkCore;
using Organization.Share.Key;
using Organization.Storage.EntityModel;

namespace Organization.Storage.Repo
{
    public class OrganizationRepository : EFRepositoryGeneric<ApiDbContext, Entity.Organization, OrganizationKey>
    {
        public OrganizationRepository(ApiDbContext context) : base(context)
        { }

        protected override string DefaultSortField => "CreatedOn";

        protected override void ConfigureRepository()
        {
            AddQuery(nameof(Entity.Organization), DbSet);
            AddQuery(nameof(OrganizationWithEmployee), DbSet.Include(entity=>entity.EmployeeList).ThenInclude(entity=>entity.Position));
        }
    }
}
