using EasyX.Data.Api.Entity;
using Organization.Share.Key;
using System;
using System.Text.Json.Serialization;

namespace Organization.Share.Model
{
    public record OrganizationModel : IKey<OrganizationKey>
    {
        public Guid? Id { get; set; }
        public string Titlte { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByName { get; set; }

        [JsonIgnore]
        public OrganizationKey Key => Id.HasValue ? new() { Id = Id.Value } : null;
    }
}
