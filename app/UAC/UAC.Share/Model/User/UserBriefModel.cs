using EasyX.Data.Api.Entity;
using System;
using System.Text.Json.Serialization;
using UAC.Share.Key;

namespace UAC.Share.Model
{
    public record UserBriefModel : IKey<UserKey>
    {
        public Guid Id { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsDeleted { get; set; }
        [JsonIgnore]
        public UserKey Key => new() { Id = Id };
    }
}
