using System;

namespace Organization.Share.Key
{
    public sealed class EmployeeKey : IEquatable<EmployeeKey>
    {
        public Guid OrganizationId { get; set; }
        public Guid PersonId { get; set; }
        public bool Equals(EmployeeKey other)
        {
            if (other == null)
            {
                return false;
            }

            return OrganizationId == other.OrganizationId && PersonId == other.PersonId;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return (obj is EmployeeKey) && Equals(obj as EmployeeKey);
        }
    }
}
