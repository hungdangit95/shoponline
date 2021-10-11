using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedKernel.Models
{
    public abstract class ValueObject
    {
        public static bool operator ==(ValueObject left, ValueObject right)
        {
            return EqualOperator(left, right);
        }

        public static bool operator !=(ValueObject left, ValueObject right)
        {
            return !(left == right);
        }

        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }
            return ReferenceEquals(left, null) || left.Equals(right);
        }

        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !(EqualOperator(left, right));
        }

        protected abstract IEnumerable<object> GetAtomicValues();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }
            var other = (ValueObject)obj;

            return GetAtomicValues().SequenceEqual(other.GetAtomicValues());
        }

        public override int GetHashCode()
        {
            return GetAtomicValues().Select(x => x != null ? x.GetHashCode() : 0).Aggregate((x, y) => x ^ y);
        }

        public virtual ValueObject Duplicate()
        {
            return MemberwiseClone() as ValueObject;
        }
    }
}
