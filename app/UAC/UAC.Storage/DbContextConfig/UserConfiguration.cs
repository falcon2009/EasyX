using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAC.Storage.Entity;

namespace UAC.Storage.DbContextConfig
{
    public static class UserConfiguration
    {
        public static void ConfigureUser(this ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<User> user = modelBuilder.Entity<User>();
            user.ToTable("user");
            user.HasKey(entity => entity.Id);
            user.Property(entity => entity.Id).ValueGeneratedOnAdd();
            user.HasMany(entity => entity.UserRoleList)
                .WithOne(entity => entity.User)
                .HasForeignKey(entity => entity.UserId);
            user.HasQueryFilter(entity => !entity.DeletedOn.HasValue);
        }
    }
}
