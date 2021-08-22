using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAC.Share.Key;
using UAC.Storage.Entity;

namespace UAC.Storage.DbContextConfig
{
    public static class UserRoleConfiguration 
    {
        public static void ConfigureUserRole(this ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<UserRole> userRole = modelBuilder.Entity<UserRole>();
            userRole.ToTable("user_role");
            userRole.HasKey(entity => new { UserId = entity.UserId, RoleId = entity.RoleId });
            userRole.HasOne(entity => entity.User)
                    .WithMany(entity => entity.UserRoleList)
                    .HasForeignKey(entity => entity.UserId);
            userRole.HasOne(entity => entity.Role)
                    .WithMany(entity => entity.UserRoleList)
                    .HasForeignKey(entity => entity.RoleId);
            userRole.HasQueryFilter(entity => !entity.DeletedOn.HasValue);
        }
    }
}
