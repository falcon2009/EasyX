using EasyX.Data.Api.Entity;
using Person.Share.Key;
using System;
using System.Text.Json.Serialization;

namespace Person.Share.Model.Person
{
    public class PersonLookupModel : IKey<PersonKey>
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }

        [JsonIgnore]
        public PersonKey Key => new() { Id = Id };
    }
}
