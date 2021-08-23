using EasyX.Data.EF;
using Organization.Share.Key;

namespace Organization.Storage.Repo
{
    public class PositionRepository : EFRepositoryGeneric<ApiDbContext, Entity.Position, PositionKey>
    {
        public PositionRepository(ApiDbContext context) : base(context)
        { }

        protected override string DefaultSortField => "Titlte";

        protected override void ConfigureRepository()
        {
            AddQuery(nameof(Entity.Position), DbSet);
        }
    }
}
