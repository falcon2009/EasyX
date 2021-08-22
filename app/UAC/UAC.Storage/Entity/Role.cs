using System.Collections.Generic;
using System.Text.Json.Serialization;

using EasyX.Data.Api.Entity;
using UAC.Share.Key;

namespace UAC.Storage.Entity
{
    public record Role : IKey<RoleKey>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<UserRole> UserRoleList { get; set; }
        [JsonIgnore]
        public RoleKey Key => new (){ Id = Id };
    }
}
