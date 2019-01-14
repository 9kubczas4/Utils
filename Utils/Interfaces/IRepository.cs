using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Utils.Algorithms.Specifications;

namespace Utils.Interfaces
{
    public interface IRepository<T> where T : class, IEntity
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null);
        T Get(Expression<Func<T, bool>> predicate);
        T Delete(Expression<Func<T, bool>> predicate);
        IQueryable<T> Query(ISpecification<T> specification, IEnumerable<SortSpecification<T>> sortSpecifications,
            int pageNumber, int pageSize);
        IQueryable<T> Query(ISpecification<T> specification);
        IQueryable<T> QueryLastPage(ISpecification<T> specification,
            IEnumerable<SortSpecification<T>> sortSpecifications, int pageSize);
        void Delete(T entity);
        void Update(T entity);
        void Update(T entity,
            IEnumerable<object> updatedSet, string propertyName);
        void Create(T entity);
        void Comit();
    }
}
