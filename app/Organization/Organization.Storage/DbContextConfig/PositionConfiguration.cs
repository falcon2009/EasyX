using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Organization.Storage.DbContextConfig
{
    public static class PositionConfiguration
    {
        public static void ConfigurePosition(this ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<Entity.Position> position = modelBuilder.Entity<Entity.Position>();
            position.ToTable("position")
                    .HasKey(entity => entity.Id);
            position.Property(entity => entity.Id)
                    .ValueGeneratedOnAdd();
            position.HasMany(entity => entity.EmployeeList)
                    .WithOne(entity => entity.Position)
                    .HasForeignKey(entity => entity.PositionId);
        }
    }
}
