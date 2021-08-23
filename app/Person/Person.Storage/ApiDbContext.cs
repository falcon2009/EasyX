using EasyX.Infra;
using Microsoft.EntityFrameworkCore;
using System;
using Person.Storage.DbContextConfig;

namespace Person.Storage
{
    public class ApiDbContext : DbContext
    {
        public DbSet<Entity.Person> PersonList { get; set; }
        public DbSet<Entity.PersonContact> PersonContactList { get; set; }
        public ApiDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder), Constant.Errors.BuilderNull);
            }

            modelBuilder.ConfigurePerson();
            modelBuilder.ConfigurePersonContact();
        }
    }
}
