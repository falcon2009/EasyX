using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Organization.Storage.DbContextConfig
{
    public static class OrganizationConfiguration
    {
        public static void ConfigureOrganization(this ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<Entity.Organization> person = modelBuilder.Entity<Entity.Organization>();
            person.ToTable("organization")
                  .HasKey(entity => entity.Id);
            person.Property(entity => entity.Id)
                  .ValueGeneratedOnAdd();
            person.HasMany(entity => entity.EmployeeList)
                    .WithOne(entity => entity.Organization)
                    .HasForeignKey(entity => entity.OrganizationId);
        }
    }
}
