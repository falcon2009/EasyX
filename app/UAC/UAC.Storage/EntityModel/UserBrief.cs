using EasyX.Data.Api.Entity;
using System;
using System.Text.Json.Serialization;
using UAC.Share.Key;

namespace UAC.Storage.EntityModel
{
    public record  UserBrief : IKey<UserKey>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsBlocked { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }

        [JsonIgnore]
        public UserKey Key => new() { Id = Id };
    }
}
