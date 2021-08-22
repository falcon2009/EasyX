using System;
using System.Text.Json.Serialization;

using UAC.Share.Key;
using EasyX.Data.Api.Entity;

namespace UAC.Storage.Entity
{
    public record UserRole : IKey<UserRoleKey>, ICreateTrackable, IDeleteTrackable
    {
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public string DeletedById { get; set; }
        public string DeletedByName { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
        [JsonIgnore]
        public UserRoleKey Key => new() { UserId = UserId, RoleId = RoleId };
    }
}
