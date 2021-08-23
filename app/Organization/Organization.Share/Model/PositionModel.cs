using EasyX.Data.Api.Entity;
using Organization.Share.Key;
using System.Text.Json.Serialization;

namespace Organization.Share.Model
{
    public record PositionModel : IKey<PositionKey>
    {
        public int? Id { get; set; }
        public string Title { get; set; }

        [JsonIgnore]
        public PositionKey Key => Id.HasValue ? new() { Id = Id.Value } : null;
    }
}
