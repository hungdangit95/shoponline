using System.Collections.Generic;
using System.Linq.Expressions;

namespace SharedKernel.Linq
{
    public class ExpressionEqualityComparer : IEqualityComparer<Expression>
    {
        public bool Equals(Expression x, Expression y)
        {
            return new ExpressionValueComparer().Compare(x, y);
        }

        public int GetHashCode(Expression obj)
        {
            return new ExpressionHashCodeResolver().GetHashCodeFor(obj);
        }
    }

    public static class ExpressionEqualityComparerExtension
    {
        public static bool IsEquals(this Expression x, Expression y)
        {
            return new ExpressionEqualityComparer().Equals(x, y);
        }

        public static bool IsEquals<T>(this Expression<T> x, Expression<T> y)
        {
            return new ExpressionEqualityComparer().Equals(x, y);
        }
    }
}
