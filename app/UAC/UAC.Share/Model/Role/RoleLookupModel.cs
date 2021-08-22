using EasyX.Data.Api.Entity;
using System.Text.Json.Serialization;
using UAC.Share.Key;

namespace UAC.Share.Model
{
    public record RoleLookupModel : IKey<RoleKey>
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        [JsonIgnore]
        public RoleKey Key => Id.HasValue ? new () { Id = Id.Value } : null;
    }
}
