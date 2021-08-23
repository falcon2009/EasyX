using System;

namespace Person.Share.Key
{
    public sealed class PersonContactKey: IEquatable<PersonContactKey>
    {
        public Guid Id { get; set; }

        public bool Equals(PersonContactKey other)
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
            return (obj is PersonContactKey) && Equals(obj as PersonContactKey);
        }
    }
}
