using System;

namespace UAC.Share.Key
{
    public sealed record RoleKey : IEquatable<RoleKey>
    {
        public int Id { get; set; }

        public bool Equals(RoleKey other)
        {
            if (other == null)
            {
                return false;
            }

            return Id == other.Id;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
