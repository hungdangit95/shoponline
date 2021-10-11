using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShopOnlineApp.Infrastructure.Interfaces
{
    public interface IRepository<T, K> where T : class
    {
        Task<T>  FindById(K id, params Expression<Func<T, object>>[] includeProperties);

        Task<T> FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        Task<IQueryable<T>>  FindAll(params Expression<Func<T, object>>[] includeProperties);

        Task<IQueryable<T>>  FindAll(Expression<Func<T, bool>> predicate, params string[] includeProperties);

        Task<IQueryable<T>> NewFindAll(Expression<Func<T, bool>> predicate,params string[] includeProperties);

        Task Add(T entity);

        Task Update(T entity);

        Task Remove(T entity);

        Task Remove(K id);

        Task RemoveMultiple(List<T> entities);

        Task SaveChanges();
    }
}
