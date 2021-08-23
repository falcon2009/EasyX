using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Person.Storage.DbContextConfig
{
    public static class PersonConfiguration
    {
        public static void ConfigurePerson(this ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<Entity.Person> person = modelBuilder.Entity<Entity.Person>();
            person.ToTable("person")
                  .HasKey(entity => entity.Id);
            person.Property(entity => entity.Id)
                  .ValueGeneratedOnAdd();
            person.HasMany(entity => entity.PersonContactList)
                  .WithOne(entity => entity.Person)
                  .HasForeignKey(entity => entity.PersonId);
        }
    }
}
