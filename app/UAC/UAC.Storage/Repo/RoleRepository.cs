using EasyX.Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAC.Share.Key;
using UAC.Storage.Entity;
using UAC.Storage.EntityModel;

namespace UAC.Storage.Repo
{
    public class RoleRepository : EFRepositoryGeneric<ApiDbContext, Role, RoleKey>
    {
        public RoleRepository(ApiDbContext context) : base(context)
        { }
        protected override string DefaultSortField => "Title";
        protected override void ConfigureRepository()
        {
            AddQuery(nameof(Role), DbSet.Include(entity => entity.UserRoleList).ThenInclude(entity => entity.User));

            static IQueryable<RoleLookup> selectRoleLookup(IQueryable<Role> query) => query.Select(entity =>
            new RoleLookup
            {
                Id = entity.Id,
                Title = entity.Title
            });
            AddQueryForModel(selectRoleLookup, DbSet);
        }
    }
}
