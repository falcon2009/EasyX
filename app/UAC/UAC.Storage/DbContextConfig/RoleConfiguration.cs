using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAC.Storage.Entity;

namespace UAC.Storage.DbContextConfig
{
    public static class RoleConfiguration
    {
        public static void ConfigureRole(this ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<Role> user = modelBuilder.Entity<Role>();
            user.ToTable("role");
            user.HasKey(entity => entity.Id);
            user.Property(entity => entity.Id).ValueGeneratedOnAdd();
            user.HasMany(entity => entity.UserRoleList)
                .WithOne(entity => entity.Role)
                .HasForeignKey(entity => entity.RoleId);
        }
    }
}
