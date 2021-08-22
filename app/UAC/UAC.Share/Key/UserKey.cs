using System;

namespace UAC.Share.Key
{
    public sealed class UserKey : IEquatable<UserKey>
    {
        public Guid Id { get; set; }

        public bool Equals(UserKey other)
        {
            if (other == null)
            {
                return false;
            }

            return Id == other.Id;
        }
    }
}
