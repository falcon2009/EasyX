using Person.Share.Enum;
using Person.Share.Key;
using System;
using System.Text.Json.Serialization;

namespace Person.Share.Model
{
    public class PersonContactModel
    {
        public Guid? Id { get; set; }
        public Guid? PersonId { get; set; }
        public ContactInfoType Type { get; set; }
        public string Value { get; set; }

        [JsonIgnore]
        public PersonContactKey Key => Id.HasValue ? new() { Id = Id.Value } : null;
    }
}
