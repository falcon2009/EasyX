using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Organization.Storage.DbContextConfig
{
    public static class EmployeeConfiguration
    {
        public static void ConfigureEmployee(this ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<Entity.Employee> person = modelBuilder.Entity<Entity.Employee>();
            person.ToTable("employee")
                  .HasKey(entity => new { entity.OrganizationId, entity.PersonId });
            person.HasOne(entity => entity.Organization)
                  .WithMany(entity => entity.EmployeeList)
                  .HasForeignKey(entity => entity.OrganizationId);
            person.HasOne(entity => entity.Position)
                  .WithMany(entity => entity.EmployeeList)
                  .HasForeignKey(entity => entity.PositionId);
        }
    }
}
