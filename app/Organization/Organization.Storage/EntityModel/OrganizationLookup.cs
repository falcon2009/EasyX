using EasyX.Data.Api.Entity;
using Organization.Share.Key;
using System;
using System.Text.Json.Serialization;

namespace Organization.Storage.EntityModel
{
    public record OrganizationLookup : IKey<OrganizationKey>
    {
        public Guid Id { get; set; }
        public string Titlte { get; set; }

        [JsonIgnore]
        public OrganizationKey Key => new() { Id = Id };
    }
}
