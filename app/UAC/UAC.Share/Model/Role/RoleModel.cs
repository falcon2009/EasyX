using EasyX.Data.Api.Entity;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UAC.Share.Key;

namespace UAC.Share.Model
{
    public record RoleModel : IKey<RoleKey>
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public List<UserModel> UserModelList { get; set; }

        [JsonIgnore]
        public RoleKey Key => Id.HasValue ? new () { Id = Id.Value } : null;
    }
}
