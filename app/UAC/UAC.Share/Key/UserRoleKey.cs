using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UAC.Share.Key
{
    public sealed record UserRoleKey : IEquatable<UserRoleKey>
    {
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
        public bool Equals(UserRoleKey other)
        {
            if (other == null)
            {
                return false;
            }

            return UserId == other.UserId && RoleId == other.RoleId;
        }
    }
}
