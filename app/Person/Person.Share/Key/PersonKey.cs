using System;

namespace Person.Share.Key
{
    public class PersonKey : IEquatable<PersonKey>
    {
        public Guid Id;
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
