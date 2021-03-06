using System;

namespace Organization.Share.Key
{
    public sealed class OrganizationKey : IEquatable<OrganizationKey>
    {
        public Guid Id { get; set; }

        public bool Equals(OrganizationKey other)
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
        public override bool Equals(object obj)
        {
            return (obj is OrganizationKey) && Equals(obj as OrganizationKey);
        }
    }
}
