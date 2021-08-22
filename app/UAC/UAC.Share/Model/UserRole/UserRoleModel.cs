using EasyX.Data.Api.Entity;
using System;
using System.Text.Json.Serialization;
using UAC.Share.Key;

namespace UAC.Share.Model
{
    public class UserRoleModel : IKey<UserRoleKey>
    {
        public Guid? UserId { get; set; }
        public int? RoleId { get; set; }
        [JsonIgnore]
        public UserRoleKey Key => UserId.HasValue && RoleId.HasValue ? new() { UserId = UserId.Value, RoleId = RoleId.Value }: null;
    }
}
