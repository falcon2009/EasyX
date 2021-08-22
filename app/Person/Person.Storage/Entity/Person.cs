using EasyX.Data.Api.Entity;
using Person.Share.Enum;
using Person.Share.Key;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Person.Storage.Entity
{
    public record Person : IKey<PersonKey>, ICreateTrackable, IUpdateTrackable, IDeleteTrackable
    {
        public Guid Id { get; set; }
        public string PersonalNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public PersonGender Gender { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string UpdatedById { get; set; }
        public string UpdatedByName { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public string DeletedById { get; set; }
        public string DeletedByName { get; set; }
        public List<PersonContact> PersonContactList { get; set; }
        [JsonIgnore]
        public PersonKey Key => new() { Id = Id };
    }
}
