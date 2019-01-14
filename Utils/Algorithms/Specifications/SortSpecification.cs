using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Utils.Algorithms.Specifications
{
    public class SortSpecification<T>
    {
        public ListSortDirection Direction { get; set; }
        public Expression<Func<T, object>> Predicate { get; set; }
    }
}
