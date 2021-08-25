using Microsoft.EntityFrameworkCore;
using Organization.Storage.DbContextConfig;

namespace Organization.Storage
{
    public class ApiDbContext : DbContext
    {
        public DbSet<Entity.Organization> OrganizationList { get; set; }
        public DbSet<Entity.Employee> EmployeeList { get; set; }
        public DbSet<Entity.Position> PositionList { get; set; }

        public ApiDbContext(DbContextOptions options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureOrganization();
            modelBuilder.ConfigureEmployee();
            modelBuilder.ConfigurePosition();
        }
    }
}
