using System;
using System.Linq.Expressions;


namespace SharedKernel.Specifications
{
    public class Specification<T> : ISpecification<T>
    {
        public Specification(Expression<Func<T,bool>> predicate)
        {
            Predicate = predicate ?? throw new ArgumentNullException();
        }
        public Expression<Func<T, bool>> Predicate { get; }

        public ISpecification<T> And(ISpecification<T> specification)
        {
            return new AndSpecification<T>(this,specification);
        }

        public ISpecification<T> Or(ISpecification<T> specification)
        {
            return new OrSpecification<T>(this, specification);
        }
    }

    public class EverythingSpecification<T> : Specification<T>
    {
        public EverythingSpecification() : base(t => true)
        {
        }
    }
}
