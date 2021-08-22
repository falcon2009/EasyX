using EasyX.Data.Api.Entity;
using System.Text.Json.Serialization;
using UAC.Share.Key;

namespace UAC.Storage.EntityModel
{
    public class RoleLookup : IKey<RoleKey>
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [JsonIgnore]
        public RoleKey Key => new() { Id = Id };
    }
}
