using System;
using System.Linq.Expressions;

namespace SharedKernel.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T,bool>> Predicate { get; }
        ISpecification<T> And(ISpecification<T> specification);
        ISpecification<T> Or(ISpecification<T> specification);
    }
}
