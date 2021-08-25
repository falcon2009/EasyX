using EasyX.Data.Api.Entity;
using Organization.Share.Key;
using System;
using System.Text.Json.Serialization;

namespace Organization.Share.Model
{
    public record OrganizationLookupModel : IKey<OrganizationKey>
    {
        public Guid Id { get; set; }
        public string Titlte { get; set; }

        [JsonIgnore]
        public OrganizationKey Key => new() { Id = Id };
    }
}
