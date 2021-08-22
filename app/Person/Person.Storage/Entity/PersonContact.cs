using EasyX.Data.Api.Entity;
using Person.Share.Enum;
using Person.Share.Key;
using System;
using System.Text.Json.Serialization;

namespace Person.Storage.Entity
{
    public record PersonContact : IKey<PersonContactKey>
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public ContactInfoType Type { get; set; }
        public string Value { get; set; }
        public Person Person { get; set; }

        [JsonIgnore]
        public PersonContactKey Key => new() { Id = Id };
    }
}
