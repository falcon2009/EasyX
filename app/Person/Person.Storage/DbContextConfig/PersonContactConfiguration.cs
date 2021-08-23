using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Person.Storage.DbContextConfig
{
    public static class PersonContactConfiguration
    {
        public static void ConfigurePersonContact(this ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<Entity.PersonContact> personContact = modelBuilder.Entity<Entity.PersonContact>();
            personContact.ToTable("person_contact")
                  .HasKey(entity => entity.Id);
            personContact.Property(entity => entity.Id)
                         .ValueGeneratedOnAdd();
            personContact.HasOne(entity => entity.Person)
                         .WithMany(entity => entity.PersonContactList)
                         .HasForeignKey(entity => entity.PersonId);
        }
    }
}
