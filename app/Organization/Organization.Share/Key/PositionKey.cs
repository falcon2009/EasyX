using System;

namespace Organization.Share.Key
{
    public sealed class PositionKey : IEquatable<PositionKey>
    {
        public int Id { get; set; }
        public bool Equals(PositionKey other)
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
            return (obj is PositionKey) && Equals(obj as PositionKey);
        }
    }
}
