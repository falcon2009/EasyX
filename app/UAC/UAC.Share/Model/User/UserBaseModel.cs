using System;
using System.Text.Json.Serialization;
using EasyX.Data.Api.Entity;
using UAC.Share.Key;

namespace UAC.Share.Model
{
    public record UserBaseModel : IKey<UserKey>
    {
        public Guid? Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsBlocked { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string UpdatedById { get; set; }
        public string UpdatedByName { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public string DeletedById { get; set; }
        public string DeletedByName { get; set; }

        [JsonIgnore]
        public UserKey Key => Id.HasValue ? new () { Id = Id.Value } : null;
    }
}
