using EasyX.Data.Core;
using EasyX.Infra;
using Microsoft.EntityFrameworkCore;
using System;
using UAC.Storage.DbContextConfig;
using UAC.Storage.Entity;

namespace UAC.Storage
{
    public class ApiDbContext : DbContext
    {
        public DbSet<User> UserList { get; set; }
        public DbSet<Role> RoleList { get; set; }
        public DbSet<UserRole> UserRoleList { get; set; }
        public ApiDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder), Constant.Errors.BuilderNull);
            }

            modelBuilder.ConfigureUser();
            modelBuilder.ConfigureRole();
            modelBuilder.ConfigureUserRole();
        }
    }
}
