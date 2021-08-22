using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using EasyX.Data.Api.Entity;
using Person.Share.Key;


namespace Person.Share.Model.Person
{
    public class PersonModel : IKey<PersonKey>
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string UpdatedById { get; set; }
        public string UpdatedByName { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public string DeletedById { get; set; }
        public string DeletedByName { get; set; }
        public List<PersonContactModel> PersonContactList { get; set; }
        [JsonIgnore]
        public PersonKey Key => Id.HasValue ? new() { Id = Id.Value } : null;
    }
}
