using EasyX.Data.EF;
using Microsoft.EntityFrameworkCore;
using Organization.Share.Key;

namespace Organization.Storage.Repo
{
    public class EmployeeRepository : EFRepositoryGeneric<ApiDbContext, Entity.Employee, EmployeeKey>
    {
        public EmployeeRepository(ApiDbContext context) : base(context)
        { }

        protected override string DefaultSortField => "CreatedOn";

        protected override void ConfigureRepository()
        {
            AddQuery(nameof(Entity.Employee), DbSet.Include(entity=>entity.Position));
        }
    }
}
