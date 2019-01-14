using System;
using System.Linq.Expressions;
using Utils.Interfaces;

namespace Utils.Algorithms.Specifications
{
    public class NotSpecification<T> : CompositeSpecification<T>
    {
        private readonly ISpecification<T> _left;

        public NotSpecification(ISpecification<T> left)
        {
            _left = left;
        }

        public override Expression<Func<T, bool>> IsSatisfiedBy()
        {
            var isSatisfiedBy = _left.IsSatisfiedBy();

            return Expression.Lambda<Func<T, bool>>(
                Expression.Not(isSatisfiedBy.Body), isSatisfiedBy.Parameters);
        }
    }
}
