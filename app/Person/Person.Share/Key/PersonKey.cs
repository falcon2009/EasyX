using System;

namespace Person.Share.Key
{
    public sealed class PersonKey : IEquatable<PersonKey>
    {
        public Guid Id { get; set; }
        public bool Equals(PersonKey other)
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
            return (obj is PersonKey) && Equals(obj as PersonKey);
        }
    }
}
