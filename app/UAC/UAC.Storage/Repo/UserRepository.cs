using System.Linq;
using Microsoft.EntityFrameworkCore;

using EasyX.Data.EF;
using UAC.Share.Key;
using UAC.Storage.Entity;
using UAC.Storage.EntityModel;

namespace UAC.Storage.Repo
{
    public class UserRepository : EFRepositoryGeneric<ApiDbContext, User, UserKey>
    {
        public UserRepository(ApiDbContext context) : base(context)
        { }

        protected override string DefaultSortField => "CreatedOn";

        protected override void ConfigureRepository()
        {
            static IQueryable<UserBase> selectUseBase(IQueryable<User> query) => query.Select(entity =>
             new UserBase
             {
                 Id = entity.Id,
                 Login = entity.Login,
                 Password = entity.Password,
                 CreatedById = entity.CreatedById,
                 CreatedByName = entity.CreatedByName,
                 CreatedOn = entity.CreatedOn,
                 UpdatedById = entity.UpdatedById,
                 UpdatedByName = entity.UpdatedByName,
                 UpdatedOn = entity.UpdatedOn,
                 DeletedById= entity.DeletedById,
                 DeletedByName = entity.DeletedByName,
                 DeletedOn = entity.DeletedOn,
                 IsBlocked = entity.IsBlocked
             });
            static IQueryable<UserBrief> selectUserBrief(IQueryable<User> query) => query.Select(entity =>
            new UserBrief
            {
                Id = entity.Id,
                DeletedOn = entity.DeletedOn,
                IsBlocked = entity.IsBlocked
            });
            AddQuery(nameof(User), DbSet.Include(entity => entity.UserRoleList).ThenInclude(entity => entity.Role));
            AddQueryForModel(selectUseBase, DbSet);
            AddQueryForModel(selectUserBrief, DbSet);
        }
    }
}
