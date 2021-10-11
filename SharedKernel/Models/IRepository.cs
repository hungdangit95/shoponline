using SharedKernel.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedKernel.Models
{
    public interface IRepository<T> where T: IAggregateRoot
    {
        Task<QueryResult<T>> QueryAsync(ISpecification<T> spec, int skip, int take, string[] sorts = null);
        Task<IList<T>> GetManyAsync(ISpecification<T> spec, string[] sorts = null);
        Task<IList<T>> GetManyAsync(ISpecification<T> spec, int skip, int take, string[] sorts = null);
        Task<IList<T>> GetAllAsync(string[] sorts = null);
        Task<T> GetSingleAsync(ISpecification<T> spec, string[] sorts = null);
        Task<T> GetByIdAsync(Guid entityId);
        Task<bool> ExistsAsync(ISpecification<T> spec);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(T entity);
        void DeleteMany(IEnumerable<T> entities);
        Task<int> CountAllAsync();
        Task<int> CountAsync(ISpecification<T> specification);
        IQueryable<T> Queryable(ISpecification<T> specification);
    }
}
